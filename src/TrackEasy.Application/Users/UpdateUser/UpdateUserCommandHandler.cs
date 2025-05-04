using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.UpdateUser;

internal sealed class UpdateUserCommandHandler(UserManager<User> userManager, TimeProvider timeProvider) : ICommandHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user is null)
        {
            throw new TrackEasyException(Codes.UserNotFound, "User not found");
        }
        
        user.UpdatePersonalData(request.FirstName, request.LastName, request.BirthDate, timeProvider);
        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            throw new TrackEasyException(Codes.UserUpdateFailed, "User update failed");
        }
    }
}