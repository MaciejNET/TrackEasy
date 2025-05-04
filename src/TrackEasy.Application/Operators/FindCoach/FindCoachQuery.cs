using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Operators.FindCoach;

public sealed record FindCoachQuery(Guid Id, Guid OperatorId) : IQuery<CoachDetailsDto?>;