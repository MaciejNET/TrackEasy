using TrackEasy.Application.Operators.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.FindOperator;

public sealed record FindOperatorQuery(Guid Id) : IQuery<OperatorDto?>;