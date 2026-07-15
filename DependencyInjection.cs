    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using TravelApp.Infrastructure.Data.Helpers;
    using TravelApp.Infrastructure.Data.Services;
    using TrevalApp.Interfaces.Services;
    using TrevalApp.Models;

    namespace TravelApp.Infrastructure.Data;

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));
            
            // ASP.NET Identity
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            
            // Redis
            // var redisConnectionString = configuration.GetSection("Redis:ConnectionString").Value ?? "localhost:6379";
            // services.AddSingleton<IConnectionMultiplexer>(_ =>
            //     ConnectionMultiplexer.Connect(redisConnectionString));
            // services.AddSingleton<ICacheService, RedisCacheService>();
            
            // Email 
            // services.AddScoped<IEmailServices, SmtpEmailService>();
            
            // Unit of Work & Repositories
            // services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            // JWT
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            //  Services
            services.AddScoped<IAuthService , AuthService>();
            services.AddScoped<IDestinationService, Destinationservice>();
            services.AddScoped<ITourService, Toursrvice>();
            services.AddScoped<IHotelService, Hotelservice>();
            services.AddScoped<IBookingService, Bookingservice>();
            return services;
        }
    }