using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Mails.Templates;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Mails;

internal sealed class EmailSender(RazorRenderer razorRenderer, IConfiguration configuration) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage, IEnumerable<EmailAttachment>? attachments = null)
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
        
        if (attachments != null)
        {
            foreach (var att in attachments)
            {
                bodyBuilder.Attachments.Add(
                    att.FileName,
                    att.Data,
                    ContentType.Parse(att.ContentType)
                );
            }
        }
        
        message.Body = bodyBuilder.ToMessageBody();
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendAccountConfirmationEmailAsync(string email, ActivateEmailModel model)
    {
        const string subject = "Account confirmation";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<ActivateEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }

    public async Task SendTwoFactorEmailAsync(string email, TwoFactorEmailModel model)
    {
        const string subject = "Two-factor authentication";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<TwoFactorEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }
    
    public async Task SendTicketNumberEmailAsync(string email, TicketNumberModel model)
    {
        const string subject = "Ticket number";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<TicketNumberEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }

    public async Task SendTicketEmailAsync(string email, TicketEmailModel model)
    {
        const string subject = "Ticket";
        var html = await razorRenderer.RenderHtmlToString<TicketEmail>();
        
        var ticket = new EmailAttachment(
            $"{model.TicketNumber}.pdf",
            model.TicketPdf,
            "application/pdf"
        );
        
        await SendEmailAsync(email, subject, html, [ticket]);
    }

    public async Task SendTicketRefundedEmailAsync(string email, TicketRefundedModel model)
    {
        const string subject = "Ticket refunded";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<TicketRefundedEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }

    public async Task SendTicketCancelledEmailAsync(string email, TicketCancelledModel model)
    {
        const string subject = "Ticket cancelled";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<TicketCancelledEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }

    public async Task SendRefundRejectedEmailAsync(string email, RefundRejectedModel model)
    {
        const string subject = "Refund request rejected";
        var parameters = new Dictionary<string, object?>()
        {
            { "Model", model }
        };
        var html = await razorRenderer.RenderHtmlToString<RefundRequestRejectedEmail>(parameters);
        await SendEmailAsync(email, subject, html);
    }
}