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
        var user = await userManager.FindByIdAsync(notification.UserId.ToString());
        if (user is null)
        {
            return;
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

        var url = configuration["frontend-url"]!;
        var model = new ActivateEmailModel(user.FirstName!, user.LastName!, user.Email!, token, url);
        await emailSender.SendAccountConfirmationEmailAsync(user.Email!, model);
    }
}