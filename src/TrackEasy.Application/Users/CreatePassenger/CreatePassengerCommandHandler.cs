using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Users.CreatePassenger;

internal sealed class CreatePassengerCommandHandler(UserManager<User> userManager, TimeProvider timeProvider) : ICommandHandler<CreatePassengerCommand>
{
    public async Task Handle(CreatePassengerCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        
        if (existingUser is not null)
        {
            throw new TrackEasyException(Codes.UserAlreadyExists, $"User with email {request.Email} already exists.");
        }
        
        var user = User.CreatePassenger(request.FirstName, request.LastName, request.Email, request.DateOfBirth, timeProvider);
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            throw new TrackEasyException(Codes.UserCreationFailed, $"User creation failed: {string.Join(", ", errors)}");
        }
        
        var addToRoleResult = await userManager.AddToRoleAsync(user, Roles.Passenger);
        if (!addToRoleResult.Succeeded)
        {
            var errors = addToRoleResult.Errors.Select(e => e.Description).ToList();
            throw new TrackEasyException(Codes.UserRoleAdditionFailed, $"User role addition failed: {string.Join(", ", errors)}");
        }
    }
}