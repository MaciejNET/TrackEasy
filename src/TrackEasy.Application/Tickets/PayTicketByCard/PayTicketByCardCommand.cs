using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.PayTicketByCard;

public sealed record PayTicketByCardCommand(
    IEnumerable<Guid> TicketIds,
    Currency Currency,
    string CardNumber,
    int CardExpMonth,
    int CardExpYear,
    string CardCvc) : ICommand;