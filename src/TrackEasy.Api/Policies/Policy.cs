using TrackEasy.Domain.Users;

namespace TrackEasy.Api.Policies;

public static class Policy
{
    public const string Passenger = "Passenger";
    public const string Manager = "Manager";
    public const string Admin = "Admin";

    public static IServiceCollection AddPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(Passenger, policy =>
            {
                policy.RequireRole(Roles.Passenger);
            });
        
        services.AddAuthorizationBuilder()
            .AddPolicy(Manager, policy =>
            {
                policy.RequireRole(Roles.Manager);
            });
        
        services.AddAuthorizationBuilder()
            .AddPolicy(Admin, policy =>
            {
                policy.RequireRole(Roles.Admin);
            });
        
        return services;
    }
}