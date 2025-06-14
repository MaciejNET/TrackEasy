using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.PayTicketByCash;

public sealed record PayTicketByCashCommand(int TicketNumber) : ICommand;