using TrackEasy.Shared.Domain.Abstractions;

namespace TrackEasy.Domain.Tickets;

public sealed record TicketCreatedForExternalUserEvent(Guid TicketId) : IDomainEvent;
