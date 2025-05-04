using TrackEasy.Mails.Abstractions.Models;

namespace TrackEasy.Mails.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);
    Task SendAccountConfirmationEmailAsync(string email, ActivateEmailModel model);
    Task SendTwoFactorEmailAsync(string email, TwoFactorEmailModel model);
}