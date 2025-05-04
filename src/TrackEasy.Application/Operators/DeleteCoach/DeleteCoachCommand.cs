using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.DeleteCoach;

public sealed record DeleteCoachCommand(Guid Id, Guid OperatorId) : ICommand;