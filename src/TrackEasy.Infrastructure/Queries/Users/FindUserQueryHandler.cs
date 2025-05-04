using Microsoft.AspNetCore.Identity;
using TrackEasy.Application.Security;
using TrackEasy.Application.Users;
using TrackEasy.Application.Users.FindUser;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Infrastructure.Queries.Users;

internal sealed class FindUserQueryHandler(UserManager<User> userManager, IUserContext userContext) : IQueryHandler<FindUserQuery, UserDto>
{
    public async Task<UserDto> Handle(FindUserQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userContext.UserId.ToString()!);
        if (user is null)
        {
            throw new TrackEasyException(Codes.UserNotFound, "User not found.");
        }

        return new UserDto(user.Id, user.FirstName!, user.LastName!, user.Email!, userContext.Role!,
            userContext.OperatorId);
    }
}