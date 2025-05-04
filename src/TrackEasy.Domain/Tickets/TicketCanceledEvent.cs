using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public sealed record TicketCanceledEvent(Guid TicketId) : IDomainEvent;