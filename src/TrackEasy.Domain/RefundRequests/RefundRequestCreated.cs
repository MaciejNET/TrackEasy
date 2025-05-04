using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.RefundRequests;

public sealed record RefundRequestCreated(Guid OperatorId) : IDomainEvent;