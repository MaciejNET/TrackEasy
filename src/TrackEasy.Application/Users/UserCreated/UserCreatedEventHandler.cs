using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using TrackEasy.Domain.Users;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.UserCreated;

internal sealed class UserCreatedEventHandler(UserManager<User> userManager, IEmailSender emailSender, IConfiguration configuration) : IDomainEventHandler<UserCreatedEvent>
{
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(notification.User);
        
        var url = configuration["frontend-url"]!;
        var model = new ActivateEmailModel(notification.User.FirstName!, notification.User.LastName!, notification.User.Email!, token, url);
        await emailSender.SendAccountConfirmationEmailAsync(notification.User.Email!, model);
    }
}