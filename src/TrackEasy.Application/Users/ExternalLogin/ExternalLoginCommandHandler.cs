using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TrackEasy.Application.Security;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.ExternalLogin;

internal sealed class ExternalLoginCommandHandler(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IJwtService jwtService,
    TimeProvider timeProvider) : ICommandHandler<ExternalLoginCommand, string>
{
    public async Task<string> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
    {
        var info = await signInManager.GetExternalLoginInfoAsync();
        if (info is null || !string.Equals(info.LoginProvider, request.Provider, StringComparison.OrdinalIgnoreCase))
        {
            throw new TrackEasyException(Codes.ExternalLoginFailed, "External login information not found.");
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new TrackEasyException(Codes.EmailNotProvided, "Email not provided by external provider.");
        }

        var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
        if (user is null)
        {
            user = User.CreatePassenger(request.FirstName, request.LastName, email, request.DateOfBirth, timeProvider);
            var createResult = await userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => e.Description).ToList();
                throw new TrackEasyException(Codes.UserCreationFailed, $"User creation failed: {string.Join(", ", errors)}");
            }

            var roleResult = await userManager.AddToRoleAsync(user, Roles.Passenger);
            if (!roleResult.Succeeded)
            {
                var errors = roleResult.Errors.Select(e => e.Description).ToList();
                throw new TrackEasyException(Codes.UserRoleAdditionFailed, $"User role addition failed: {string.Join(", ", errors)}");
            }

            var loginResult = await userManager.AddLoginAsync(user, info);
            if (!loginResult.Succeeded)
            {
                var errors = loginResult.Errors.Select(e => e.Description).ToList();
                throw new TrackEasyException(Codes.ExternalLoginFailed, $"External login addition failed: {string.Join(", ", errors)}");
            }
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        await signInManager.UpdateExternalAuthenticationTokensAsync(info);

        var roles = await userManager.GetRolesAsync(user);
        return jwtService.GenerateToken(user.Id, user.Email!, roles.First(), true, null);
    }
}

