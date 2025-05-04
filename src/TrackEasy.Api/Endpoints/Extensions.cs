using System.Reflection;
using TrackEasy.Api.Filters;

namespace TrackEasy.Api.Endpoints;

public static class Extensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var rootGroup = app.MapGroup("")
            .AddEndpointFilter<Global2FAFilter>();
        
        var endpointTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t is { IsClass: true } &&
                        t.GetInterfaces().Any(i => i == typeof(IEndpoints)));

        foreach (var type in endpointTypes)
        {
            var method = type.GetMethod(nameof(IEndpoints.MapEndpoints), 
                BindingFlags.Public | BindingFlags.Static,
                [typeof(RouteGroupBuilder)]);
            
            method?.Invoke(null, [rootGroup]);
        }

        return app;
    }
}