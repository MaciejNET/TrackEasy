using Microsoft.AspNetCore.Identity;
using TrackEasy.Domain.Operators;
using TrackEasy.Domain.Users;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Operators.AddManager;

internal sealed class AddManagerCommandHandler(
    IOperatorRepository operatorRepository,
    UserManager<User> userManager,
    TimeProvider timeProvider)
    : ICommandHandler<AddManagerCommand>
{
    public async Task Handle(AddManagerCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorRepository.FindAsync(request.OperatorId, cancellationToken);
        
        if (@operator is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Operator with id: {request.OperatorId} does not exists.");
        }
        
        var manager = @operator.AddManager(request.FirstName, request.LastName, request.Email, request.DateOfBirth, timeProvider);
        var user = manager.User;
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new TrackEasyException(Users.Codes.UserCreationFailed, 
                $"User creation failed: {string.Join(", ", result.Errors.Select(x => x.Description))}");
        }
        
        result = await userManager.AddToRoleAsync(user, Roles.Manager);
        if (!result.Succeeded)
        {
            throw new TrackEasyException(Users.Codes.UserRoleAdditionFailed, 
                $"User role assignment failed: {string.Join(", ", result.Errors.Select(x => x.Description))}");
        }
        
        await operatorRepository.SaveChangesAsync(cancellationToken);
    }
}