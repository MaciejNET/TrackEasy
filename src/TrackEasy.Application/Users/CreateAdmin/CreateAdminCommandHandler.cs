using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.CreateAdmin;

internal sealed class CreateAdminCommandHandler(UserManager<User> userManager, TimeProvider timeProvider) : ICommandHandler<CreateAdminCommand>
{
    public async Task Handle(CreateAdminCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new TrackEasyException(Codes.UserAlreadyExists, $"User with email {request.Email} already exists.");
        }

        var user = User.CreateAdmin(
            request.FirstName,
            request.LastName,
            request.Email,
            request.DateOfBirth,
            timeProvider);

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            throw new TrackEasyException(Codes.UserCreationFailed, 
                $"Admin creation failed: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
        }

        var roleResult = await userManager.AddToRoleAsync(user, Roles.Admin);
        if (!roleResult.Succeeded)
        {
            throw new TrackEasyException(Codes.UserRoleAdditionFailed,
                $"Admin role addition failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
    }
}