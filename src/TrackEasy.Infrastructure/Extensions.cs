using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using TrackEasy.Application.Security;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Users;
using TrackEasy.Infrastructure.Behaviors;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Infrastructure.Exceptions;
using TrackEasy.Infrastructure.Security;
using TrackEasy.Infrastructure.Services;
using TrackEasy.Mails;
using TrackEasy.Pdf;
using TrackEasy.Shared.Files;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddMails();
        services.AddPdf();
        services.AddFiles();
        services.AddExceptionHandlers();
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        services.AddScoped<IJwtService, JwtService>();
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
            .AddSignInManager();
        
        services.ConfigureExternalCookie(options => {
            options.Cookie.Name = ".AspNetCore.Identity.External";
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(IdentityConstants.ApplicationScheme)
        .AddCookie(IdentityConstants.ExternalScheme)
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        })
        .AddGoogle(options =>
        {
            options.ClientId = configuration["google-clientid"]!;
            options.ClientSecret = configuration["google-clientsecret"]!;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.CallbackPath = "/users/external/google/callback";
            options.CorrelationCookie.SameSite = SameSiteMode.Lax;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.SaveTokens = true;
        })
        .AddMicrosoftAccount(options =>
        {
            options.ClientId = configuration["microsoft-clientid"]!;
            options.ClientSecret = configuration["microsoft-clientsecret"]!;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.CallbackPath = "/users/external/microsoft/callback";
            options.CorrelationCookie.SameSite = SameSiteMode.Lax;
            options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
            options.SaveTokens = true;

            options.AuthorizationEndpoint =
                MicrosoftAccountDefaults.AuthorizationEndpoint.Replace(
                    "/common/", "/consumers/");
            options.TokenEndpoint =
                MicrosoftAccountDefaults.TokenEndpoint.Replace(
                    "/common/", "/consumers/");
        });

        services.AddScoped<IDistributedCache, MemoryDistributedCache>();
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = false;
            options.Cookie.IsEssential = true;
        });
        
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        
        services.AddAuthorization();
        
        services.AddDataProtection();

        services.AddScoped<HtmlRenderer>();
        services.AddScoped<RazorRenderer>();
        
        services.AddDbContext<TrackEasyDbContext>(options =>
            options.UseAzureSql(
                configuration.GetValue<string>("cs-application"),
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

        services.AddSingleton(_ =>
        {
            var secretKey = configuration.GetValue<string>("stripe-secret-key");
            return new StripeClient(secretKey);
        });

        services.AddTransient<IConnectionSeatsService, ConnectionSeatsService>();
        services.AddTransient<ICurrencyService, CurrencyService>();
        services.AddTransient<ITicketPriceService, TicketPriceService>();
        services.AddTransient<INotificationService, NotificationService>();
        services.AddMemoryCache();
        services.AddRepositories();
        services.AddHostedService<SeedData>();
        services.AddHostedService<OutboxProcessor>();
        services.AddSignalR();
           
        return services;
    }
    
    public static WebApplication UseInfrastructure(this WebApplication app)
    {
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