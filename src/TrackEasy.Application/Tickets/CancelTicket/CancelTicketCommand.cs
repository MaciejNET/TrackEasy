using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.CancelTicket;

public sealed record CancelTicketCommand(Guid TicketId) : ICommand;