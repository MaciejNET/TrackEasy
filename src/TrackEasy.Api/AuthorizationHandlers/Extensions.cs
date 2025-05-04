using Microsoft.AspNetCore.Authorization;
using TrackEasy.Domain.Users;

namespace TrackEasy.Api.AuthorizationHandlers;

public static class Extensions
{
    public static RouteHandlerBuilder RequireAdminAccess(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(policy => policy
            .RequireRole(Roles.Admin));
    }
    
    public static RouteHandlerBuilder RequireManagerAccess(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(policy => policy
            .AddRequirements(new OperatorAccessRequirement())
            .RequireRole(Roles.Manager));
    }
    
    public static RouteHandlerBuilder RequirePassengerAccess(this RouteHandlerBuilder builder)
    {
        return builder.RequireAuthorization(policy => policy
            .RequireRole(Roles.Admin));
    }
    
    public static IServiceCollection AddAuthorizationHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationHandler, OperatorAccessHandler>();
        return services;
    }
}