using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public sealed record TicketPayedEvent(Guid TicketId) : IDomainEvent;