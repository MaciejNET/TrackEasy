using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.FindTicket;

public sealed record FindTicketQuery(Guid TicketId) : IQuery<TicketDetailsDto?>;