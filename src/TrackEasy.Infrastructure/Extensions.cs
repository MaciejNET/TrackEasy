using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TrackEasy.Domain.Users;
using TrackEasy.Infrastructure.Behaviors;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Infrastructure.Exceptions;
using TrackEasy.Mails;

namespace TrackEasy.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddMails();
        services.AddExceptionHandlers();
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
            options.UseAzureSql(
                services.BuildServiceProvider().GetRequiredService<IConfiguration>()
                    .GetConnectionString("DefaultConnection"),
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(TrackEasyDbContext).Assembly.FullName);
                    sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory");
                }));

        services.AddScoped<DomainEventDispatcher>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
            cfg.AddOpenBehavior(typeof(DomainEventBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionalBehavior<,>));
        });
           
        services.AddRepositories();
        //services.AddHostedService<SeedData>();
           
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
        app.MapIdentityApi<User>();
        app.UseExceptionHandlers();
        
        return app;
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        foreach (var type in assembly.GetTypes().Where(type => !type.IsAbstract))
        {
            var interfaces = type.GetInterfaces();
            foreach (var @interface in interfaces)
            {
                if (@interface.Name.EndsWith("Repository"))
                {
                    services.AddScoped(@interface, type);
                }
            }
        }
    }
}