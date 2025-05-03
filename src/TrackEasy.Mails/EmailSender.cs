using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace TrackEasy.Mails;

internal sealed class EmailSender(IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var smtpHost = configuration["smtp-host"];
        var smtpPort = int.Parse(configuration["smtp-port"]!);
        var smtpUser = configuration["smtp-username"];
        var smtpPassword = configuration["smtp-password"];
        var smtpFrom = configuration["smtp-fromemail"];
        
        using var client = new SmtpClient();
        await client.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(smtpUser, smtpPassword);
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtpFrom, smtpFrom));
        message.To.Add(new MailboxAddress(email, email));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = bodyBuilder.ToMessageBody();
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}