using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Ticket;

public sealed record TicketRefundedEvent(Guid TicketId) : IDomainEvent;