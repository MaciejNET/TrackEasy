using Microsoft.AspNetCore.Identity;
using TrackEasy.Application.Security;
using TrackEasy.Domain.Managers;
using TrackEasy.Domain.Users;
using TrackEasy.Mails.Abstractions;
using TrackEasy.Mails.Abstractions.Models;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.GenerateToken;

internal sealed class GenerateTokenCommandHandler(
    UserManager<User> userManager,
    IJwtService jwtService,
    IManagerRepository managerRepository,
    IEmailSender emailSender)
    : ICommandHandler<GenerateTokenCommand, string>
{
    public async Task<string> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new TrackEasyException(Codes.InvalidCredentials, "Invalid email or password");
        }

        if (!user.EmailConfirmed)
        {
            throw new TrackEasyException(Codes.EmailNotConfirmed, "Email address not confirmed");
        }
        
        var twoFactorVerified = !user.TwoFactorEnabled;
        if (user.TwoFactorEnabled)
        {
            await GenerateTwoFactor(user);
        }

        var roles = await userManager.GetRolesAsync(user);
        Guid? operatorId = null;
        if (roles.Contains(Roles.Manager))
        {
            var manager = await managerRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if (manager == null)
            {
                throw new TrackEasyException(Codes.ManagerNotFound, "Manager not found");
            }
            
            operatorId = manager.OperatorId;
        }
        
        return jwtService.GenerateToken(user.Id, user.Email!, roles.First(), twoFactorVerified, operatorId);
    }

    private async Task GenerateTwoFactor(User user)
    {
        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        if (token is null)
        {
            throw new TrackEasyException(Codes.TokenGenerationFailed, "Token generation failed");
        }
        
        var model = new TwoFactorEmailModel(user.FirstName!, user.LastName!, token);
        await emailSender.SendTwoFactorEmailAsync(user.Email!, model);
    }
}