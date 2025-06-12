using TrackEasy.Application.Services;
using TrackEasy.Application.Tickets.BuyTicket;
using TrackEasy.Application.Tickets.CalculateTicketPrice;
using TrackEasy.Application.Shared;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Shared;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class CalculateTicketPriceQueryHandler(
    IConnectionRepository connectionRepository,
    IStationRepository stationRepository,
    IDiscountCodeRepository discountCodeRepository,
    IDiscountRepository discountRepository,
    ITicketPriceService ticketPriceService,
    TimeProvider timeProvider)
    : IQueryHandler<CalculateTicketPriceQuery, MoneyDto>
{
    public async Task<MoneyDto> Handle(CalculateTicketPriceQuery request, CancellationToken cancellationToken)
    {
        if (request.Connections is null || !request.Connections.Any())
            throw new TrackEasyException(SharedCodes.InvalidInput, "At least one connection is required.");

        var discountCode = await ValidateDiscountCodeAsync(request.DiscountCodeId, discountCodeRepository, timeProvider, cancellationToken);

        Money? totalPrice = null;

        foreach (var dto in request.Connections)
        {
            var connection = await connectionRepository.FindByIdAsync(dto.Id, cancellationToken)
                             ?? throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection {dto.Id} was not found.");

            if (!connection.IsConnectionRunning(dto.ConnectionDate))
                throw new TrackEasyException(SharedCodes.InvalidInput, $"Connection {dto.Id} does not run on {dto.ConnectionDate:yyyy-MM-dd}.");

            var (start, end) = await LoadAndValidateStationsAsync(dto.StartStationId, dto.EndStationId, connection, stationRepository, cancellationToken);

            var pricing = await ticketPriceService.CalculateAsync(
                connection,
                request.Passengers,
                start.Id,
                end.Id,
                discountCode,
                dto.ConnectionDate,
                cancellationToken);

            if (totalPrice is null)
            {
                totalPrice = pricing.Price;
            }
            else
            {
                if (totalPrice.Currency != pricing.Price.Currency)
                    throw new TrackEasyException(SharedCodes.InvalidInput, "Connections use different currencies.");
                totalPrice += pricing.Price;
            }
        }

        totalPrice ??= new Money(0, Currency.PLN);

        return new MoneyDto(totalPrice.Amount, totalPrice.Currency);
    }

    private static async Task<DiscountCode?> ValidateDiscountCodeAsync(
        Guid? codeId,
        IDiscountCodeRepository repo,
        TimeProvider tp,
        CancellationToken ct)
    {
        if (codeId is null) return null;

        var code = await repo.GetByIdAsync(codeId.Value, ct)
                   ?? throw new TrackEasyException(SharedCodes.EntityNotFound, $"Discount code {codeId} was not found.");

        if (!code.IsActive(tp))
            throw new TrackEasyException(SharedCodes.InvalidInput, "The provided discount code is not active.");

        return code;
    }

    private static async Task<(Station start, Station end)> LoadAndValidateStationsAsync(
        Guid startId, Guid endId, Connection connection,
        IStationRepository repo, CancellationToken ct)
    {
        var start = await repo.GetByIdAsync(startId, ct)
                    ?? throw new TrackEasyException(SharedCodes.EntityNotFound, $"Start station {startId} not found.");
        var end = await repo.GetByIdAsync(endId, ct)
                    ?? throw new TrackEasyException(SharedCodes.EntityNotFound, $"End station {endId} not found.");

        if (start.Id == end.Id)
            throw new TrackEasyException(SharedCodes.InvalidInput, "Start and end stations cannot be the same.");

        if (connection.Stations.All(s => s.StationId != start.Id) || connection.Stations.All(s => s.StationId != end.Id))
            throw new TrackEasyException(SharedCodes.InvalidInput, "Start and end stations must belong to the connection.");

        return (start, end);
    }
}

