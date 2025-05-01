using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Ticket;

public sealed record TicketCanceledEvent(Guid TicketId) : IDomainEvent;