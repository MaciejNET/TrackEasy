using Microsoft.Extensions.DependencyInjection;
using Refit;
using TrackEasy.Application.Api;

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
        
        services.AddRefitClient<ICurrencyFreaksApi>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://api.currencyfreaks.com");
            });

        return services;
    }
}