using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.DeleteUser;

internal sealed class DeleteUserCommandHandler(UserManager<User> userManager) : ICommandHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            throw new TrackEasyException(Codes.UserNotFound, "User not found");
        }

        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            throw new TrackEasyException(Codes.UserDeleteFailed, "User delete failed");
        }
    }
}