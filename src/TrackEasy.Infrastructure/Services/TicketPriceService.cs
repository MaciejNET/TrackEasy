using TrackEasy.Application.Services;
using TrackEasy.Application.Tickets.BuyTicket;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Shared;
using TrackEasy.Shared.Exceptions;
using Codes = TrackEasy.Application.Tickets.Codes;

namespace TrackEasy.Infrastructure.Services;

internal sealed class TicketPriceService(IDiscountRepository discountRepository, IConnectionSeatsService seatsService) : ITicketPriceService
{
    public async Task<TicketPriceResult> CalculateAsync(Connection connection, IEnumerable<PersonDto> passengers, Guid startStationId, Guid endStationId,
        DiscountCode? discountCode, DateOnly connectionDate, CancellationToken cancellationToken)
    {
        List<int>? reservedSeats = null;
        passengers = passengers.ToList();
        
        if (connection.NeedsSeatReservation)
        {
            var available = await seatsService.GetAvailableSeatsAsync(
                connection.Id, connectionDate, startStationId, endStationId, cancellationToken);

            if (available is null)
            {
                throw new TrackEasyException(Codes.SeatNotAvailable,
                    "Unable to determine seat availability for the selected connection.");
            }

            var passengersCount = passengers.Count();
            if (available.Count < passengersCount)
            {
                throw new TrackEasyException(Codes.SeatNotAvailable,
                    "No available seats for the selected connection.");
            }

            reservedSeats = available.Take(passengersCount).ToList();
        }

        var currency = connection.PricePerKilometer.Currency;
        var totalPrice = new Money(0, currency);

        foreach (var passenger in passengers)
        {
            var discount = passenger.DiscountId.HasValue
                ? await discountRepository.GetByIdAsync(passenger.DiscountId.Value, cancellationToken)
                : null;

            if (passenger.DiscountId.HasValue && discount is null)
                throw new TrackEasyException(SharedCodes.EntityNotFound,
                    $"Discount {passenger.DiscountId} not found.");

            totalPrice += connection.CalculatePrice(startStationId, endStationId, discount);
        }

        if (discountCode is not null)
        {
            totalPrice -= discountCode.CalculateDiscount(totalPrice);
        }

        return new TicketPriceResult(totalPrice, reservedSeats);
    }
}