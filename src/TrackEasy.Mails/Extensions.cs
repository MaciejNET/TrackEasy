using Microsoft.Extensions.DependencyInjection;
using TrackEasy.Mails.Abstractions;

namespace TrackEasy.Mails;

public static class Extensions
{
    public static IServiceCollection AddMails(this IServiceCollection services)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }
}