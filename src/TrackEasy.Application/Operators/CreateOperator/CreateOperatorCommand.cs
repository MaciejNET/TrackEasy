using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.CreateOperator;

public sealed record CreateOperatorCommand(string Name, string Code) : ICommand<Guid>;