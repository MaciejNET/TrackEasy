using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.DeleteOperator;

public sealed record DeleteOperatorCommand(Guid Id) : ICommand;