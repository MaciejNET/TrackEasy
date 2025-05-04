using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.AddManager;

public sealed record AddManagerCommand(
    Guid OperatorId,
    string FirstName,
    string LastName,
    string Email,
    DateOnly DateOfBirth,
    string Password
    ) : ICommand;