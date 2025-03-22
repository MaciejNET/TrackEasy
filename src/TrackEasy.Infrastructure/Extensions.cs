using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrackEasy.Domain.Users;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Mails;

namespace TrackEasy.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMails();
        services.AddIdentityCore<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;
            options.User.RequireUniqueEmail = true;
            options.Tokens.EmailConfirmationTokenProvider = "Email";
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<TrackEasyDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<EmailTokenProvider<User>>("Email")
            .AddApiEndpoints();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
            options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
        })
        .AddCookie(IdentityConstants.BearerScheme)
        .AddJwtBearer(options =>
        {
            var key = Encoding.ASCII.GetBytes(services.BuildServiceProvider().GetRequiredService<IConfiguration>()["Jwt:Key"]!);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["Jwt:Issuer"],
                ValidAudience = services.BuildServiceProvider().GetRequiredService<IConfiguration>()["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });
        
        services.AddAuthorization();
        
        services.AddDataProtection();
        
        services.AddDbContext<TrackEasyDbContext>(options =>
            options.UseSqlServer(
                services.BuildServiceProvider().GetRequiredService<IConfiguration>()
                    .GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                    sqlOptions.MigrationsAssembly(typeof(TrackEasyDbContext).Assembly.FullName);
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "database/migrations");
                }));
        
           services.AddMediatR(cfg =>
           {
               cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
           });
           
           services.AddRepositories();
           
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.MapIdentityApi<User>();
        
        return app;
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan =>
            scan.FromAssemblies(typeof(Extensions).Assembly)
                .AddClasses(x => x.Where(c => c.Name.EndsWith("Repository")))
                .AsMatchingInterface());
    }
}