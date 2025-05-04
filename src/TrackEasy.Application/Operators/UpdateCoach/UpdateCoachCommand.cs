using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.UpdateCoach;

public sealed record UpdateCoachCommand(Guid Id, Guid OperatorId, string Name, IEnumerable<int> SeatsNumbers) : ICommand;