using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.UpdateOperator;

public sealed record UpdateOperatorCommand(Guid Id, string Name, string Code) : ICommand;