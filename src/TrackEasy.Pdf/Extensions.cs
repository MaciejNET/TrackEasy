using Microsoft.Extensions.DependencyInjection;
using TrackEasy.Pdf.Abstractions;

namespace TrackEasy.Pdf;

public static class Extensions
{
    public static IServiceCollection AddPdf(this IServiceCollection services)
    {
        services.AddTransient<IPdfService, PdfService>();
        return services;
    }
}