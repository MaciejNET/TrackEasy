using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.RefundRequests;

public sealed record RefundRejectedEvent(Guid Id, Guid TicketId) : IDomainEvent;