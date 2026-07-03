using System.Security.Claims;

namespace TravelApp.Infrastructure.Data.Helpers;

public static class Claimsprincipalextensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) 
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? throw new UnauthorizedAccessException("Không tim thấy thông tin người dùng trong token");
        return Guid.Parse(idClaim);
    }
    public static string? GetUserRole(this ClaimsPrincipal user) => user.FindFirst(ClaimTypes.Role)?.Value;
}