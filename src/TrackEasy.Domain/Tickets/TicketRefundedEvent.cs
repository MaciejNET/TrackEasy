using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public sealed record TicketRefundedEvent(Guid TicketId) : IDomainEvent;