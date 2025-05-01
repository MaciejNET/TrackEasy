using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Ticket;

public sealed record TicketPayedEvent(Guid TicketId) : IDomainEvent;