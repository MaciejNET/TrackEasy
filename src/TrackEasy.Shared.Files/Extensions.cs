using Microsoft.Extensions.DependencyInjection;
using TrackEasy.Shared.Files.Abstractions;

namespace TrackEasy.Shared.Files;

public static class Extensions
{
    public static IServiceCollection AddFiles(this IServiceCollection services)
    {
        services.AddScoped<IBlobService, BlobService>();

        return services;
    }
}