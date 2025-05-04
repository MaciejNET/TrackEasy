using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.AddCoach;

public sealed record AddCoachCommand(Guid OperatorId, string Code, IEnumerable<int> SeatsNumbers) : ICommand;