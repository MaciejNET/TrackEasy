using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TrackEasy.Mails;

public static class Extensions
{
    public static IServiceCollection AddMails(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
}