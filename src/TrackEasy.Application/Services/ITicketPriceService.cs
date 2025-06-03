using TrackEasy.Application.Tickets.BuyTicket;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;

namespace TrackEasy.Application.Services;

public interface ITicketPriceService
{
    Task<TicketPriceResult> CalculateAsync(
        Connection connection,
        IEnumerable<PersonDto> passengers,
        Guid startStationId,
        Guid endStationId,
        DiscountCode? discountCode,
        DateOnly connectionDate,
        CancellationToken cancellationToken);
}