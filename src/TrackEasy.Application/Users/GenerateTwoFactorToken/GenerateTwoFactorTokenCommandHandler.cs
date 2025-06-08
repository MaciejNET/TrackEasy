using Microsoft.AspNetCore.Identity;
using TrackEasy.Application.Security;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.GenerateTwoFactorToken;

internal sealed class GenerateTwoFactorTokenCommandHandler(UserManager<User> userManager, IJwtService jwtService, IUserContext userContext)
    : ICommandHandler<GenerateTwoFactorTokenCommand, string>
{
    public async Task<string> Handle(GenerateTwoFactorTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userContext.UserId.ToString()!);
        if (user == null)
        {
            throw new TrackEasyException(Codes.UserNotFound, "User not found");
        }
        
        var token = await userManager.VerifyTwoFactorTokenAsync(user, "Email", request.Code);
        if (!token)
        {
            throw new TrackEasyException(Codes.InvalidToken, "Invalid token");
        }

        var roles = await userManager.GetRolesAsync(user);
        
        return jwtService.GenerateToken(user.Id, user.Email!, roles.First(), true, userContext.OperatorId);
    }
}