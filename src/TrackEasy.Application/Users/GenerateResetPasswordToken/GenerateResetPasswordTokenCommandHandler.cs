using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.GenerateResetPasswordToken;

internal sealed class GenerateResetPasswordTokenCommandHandler(UserManager<User> userManager) : ICommandHandler<GenerateResetPasswordTokenCommand, string>
{
    public async Task<string> Handle(GenerateResetPasswordTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new TrackEasyException(Codes.InvalidCredentials, "Invalid email or password");
        }
        
        if (!user.EmailConfirmed)
        {
            throw new TrackEasyException(Codes.EmailNotConfirmed, "Email address not confirmed");
        }
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        if (token is null)
        {
            throw new TrackEasyException(Codes.TokenGenerationFailed, "Token generation failed");
        }
        
        return token;
    }
}