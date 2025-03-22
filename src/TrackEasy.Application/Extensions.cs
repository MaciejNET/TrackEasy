using Microsoft.Extensions.DependencyInjection;

namespace TrackEasy.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Extensions).Assembly);
        });

        services.AddSingleton(TimeProvider.System);

        return services;
    }
}