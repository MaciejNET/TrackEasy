using TrackEasy.Mails.Abstractions.Models;

namespace TrackEasy.Mails.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage, IEnumerable<EmailAttachment>? attachments = null);
    Task SendAccountConfirmationEmailAsync(string email, ActivateEmailModel model);
    Task SendTwoFactorEmailAsync(string email, TwoFactorEmailModel model);
    Task SendTicketNumberEmailAsync(string email, TicketNumberModel model);
    Task SendTicketEmailAsync(string email, TicketEmailModel model);
    Task SendTicketRefundedEmailAsync(string email, TicketRefundedModel model);
    Task SendTicketCancelledEmailAsync(string email, TicketCancelledModel model);
    Task SendRefundRejectedEmailAsync(string email, RefundRejectedModel model);
}