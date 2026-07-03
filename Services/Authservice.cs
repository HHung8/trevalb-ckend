using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using TravelApp.Infrastructure.Data.Exceptions;
using TravelApp.Infrastructure.Data.Helpers;
using TrevalApp.DTOs.Auth;
using TrevalApp.Interfaces.Services;
using TrevalApp.Models;

namespace TravelApp.Infrastructure.Data.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly JwtSettings _jwtSettings;
    
    public AuthService(
        AppDbContext context,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenGenerator jwtGenerator,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtGenerator = jwtGenerator;
        _jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
    }
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    { 
        var tempUserForHashing = new User();
        var passwordHash = _passwordHasher.HashPassword(tempUserForHashing, dto.Password);
        const string sql = @"SELECT * FROM register_user({0}, {1}, {2}, {3})";
        UserResult result;
        try
        {
            result = await _context.Database
                .SqlQueryRaw<UserResult>(sql, dto.Email, dto.FullName, passwordHash, dto.Phone).FirstAsync();
        }
        catch (Exception ex) when(ex.Message == "EMAIL_ALREADY_EXISTS")
        {
            throw new ConflictException("Email này đã được sử dụng.");
        }

        var user = MapToUser(result);
        return await BuildAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {

        const string sql = @"SELECT * FROM get_user_for_login({0})";
        var result = await _context.Database.SqlQueryRaw<UserLoginResult>(sql, dto.Email).FirstOrDefaultAsync();
        if (result == null || !result.IsActive)
            throw new UnauthorizedException("Email or password is incorrect.");
        var user = MapToUser(result);
        var verifyResult = _passwordHasher.VerifyHashedPassword(user, result.PasswordHash, dto.Password);
        if(verifyResult == PasswordVerificationResult.Failed) 
            throw new UnauthorizedException("Email or password is incorrect.");
        return await BuildAuthResponseAsync(user);
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        const string sql = @"SELECT * FROM get_user_by_refresh_token({0})";
        var result = await _context.Database.SqlQueryRaw<UserResult>(sql, refreshToken).FirstOrDefaultAsync();
        if(result == null) 
            throw new UnauthorizedException("Refresh token is incorrect.");
        var user = MapToUser(result);
        return await BuildAuthResponseAsync(user);
    }       

    public async Task LogoutAsync(Guid userId)
    {
        const string sql = @"SELECT logout_user({0})";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(sql, userId); 
        }
        catch (PostgresException ex) when(ex.MessageText == "USER_NOT_FOUND")
        {
            throw new NotFoundException("User", userId);
        }
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto dto)
    {
        const string getHashSql = @"SELECT * FROM get_user_password_hash({0})";
        var hashResult = await _context.Database.SqlQueryRaw<PasswordHashResult>(getHashSql, userId)
            .FirstOrDefaultAsync();
        if(hashResult == null)
            throw new NotFoundException("User", userId);
        var tempUser = new User { Id = userId };
        var verifyResult = _passwordHasher.VerifyHashedPassword(tempUser, hashResult.PasswordHash, dto.CurrentPassword);
        if(verifyResult == PasswordVerificationResult.Failed)
            throw new BadRequestException("Current password is incorrect.");
        var newHash = _passwordHasher.HashPassword(tempUser, dto.NewPassword);
        const string updateSql = @"SELECT update_user_password({0},{1}) ";
        try
        {
            await _context.Database.ExecuteSqlRawAsync(updateSql, userId, newHash);
        }
        catch (Exception ex)
        {
            throw new NotFoundException("User", userId);   
        }
        return true;
    }

    private async Task<AuthResponseDto> BuildAuthResponseAsync(User user)
    {
        var (accessToken, expiresAt) = _jwtGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtGenerator.GenerateRefreshToken();
        var refreshExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
 
        const string sql = @"
            UPDATE users
            SET ""RefreshToken"" = {0}, ""RefreshTokenExpiry"" = {1}
            WHERE ""Id"" = {2}";
 
        await _context.Database.ExecuteSqlRawAsync(sql, refreshToken, refreshExpiry, user.Id);
    
        var profile = new UserProfileDto(user.Id, user.FullName, user.Email!, user.PhoneNumber, user.AvatarUrl, user.Role);
        return new AuthResponseDto(accessToken, refreshToken, expiresAt, profile);
    }

    private static User MapToUser(UserResult r) => new()
    {
        Id = r.Id,
        FullName = r.FullName,
        Email = r.Email,    
        PhoneNumber = r.PhoneNumber,
        Role = r.Role,
        AvatarUrl = r.AvatarUrl
    };
    
    private static User MapToUser(UserLoginResult r) => new()
    {
        Id = r.Id,
        FullName = r.FullName,
        Email = r.Email,
        PhoneNumber = r.PhoneNumber,
        Role = r.Role,
        AvatarUrl = r.AvatarUrl
    };
    
}