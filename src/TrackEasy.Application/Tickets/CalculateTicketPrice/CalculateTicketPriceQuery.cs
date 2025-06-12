using TrackEasy.Application.Tickets.BuyTicket;
using TrackEasy.Application.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Tickets.CalculateTicketPrice;

public sealed record CalculateTicketPriceQuery(
    IEnumerable<PersonDto> Passengers,
    Guid? DiscountCodeId,
    IEnumerable<TicketConnectionDto> Connections
) : IQuery<MoneyDto>;

