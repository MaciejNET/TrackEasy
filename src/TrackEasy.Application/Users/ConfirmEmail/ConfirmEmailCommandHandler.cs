using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler(UserManager<User> userManager) : ICommandHandler<ConfirmEmailCommand>
{
    public async Task Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new TrackEasyException(Codes.UserNotFound, "User not found");
        }
        
        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            throw new TrackEasyException(Codes.InvalidToken, "Invalid token");
        }
    }
}