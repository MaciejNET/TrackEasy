using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TrackEasy.Domain.Stations;
using TrackEasy.Infrastructure.Repositories;

namespace TrackEasy.Infrastructure.Exceptions;

public static class Extensions
{
    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<TrackEasyExceptionHandler>();
        services.AddExceptionHandler<ValidationExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddScoped<ICityRepository, CityRepository>();

        return services;
    }

    public static IApplicationBuilder UseExceptionHandlers(this IApplicationBuilder app)
    {
        app.UseExceptionHandler();
        
        return app;
    }
}