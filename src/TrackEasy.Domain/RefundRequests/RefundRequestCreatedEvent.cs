using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.RefundRequests;

public sealed record RefundRequestCreatedEvent(Guid Id, Guid OperatorId) : IDomainEvent;