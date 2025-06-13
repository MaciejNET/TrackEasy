using TrackEasy.Application.Security;
using TrackEasy.Application.Services;
using TrackEasy.Domain.Connections;
using TrackEasy.Domain.DiscountCodes;
using TrackEasy.Domain.Discounts;
using TrackEasy.Domain.Stations;
using TrackEasy.Domain.Tickets;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Application.Tickets.BuyTicket;

internal sealed class BuyTicketCommandHandler(
    ITicketRepository ticketRepository,
    IConnectionRepository connectionRepository,
    IStationRepository stationRepository,
    IDiscountCodeRepository discountCodeRepository,
    IDiscountRepository discountRepository,
    IUserContext userContext,
    ITicketPriceService ticketPriceService,
    TimeProvider timeProvider
) : ICommandHandler<BuyTicketCommand, IReadOnlyCollection<Guid>>
{
    public async Task<IReadOnlyCollection<Guid>> Handle(BuyTicketCommand request,
        CancellationToken ct)
    {
        if (request.Connections is null || !request.Connections.Any())
            throw new TrackEasyException(SharedCodes.InvalidInput,
                "At least one connection is required.");

        var discountCode = await ValidateDiscountCodeAsync(
            request.DiscountCodeId, discountCodeRepository, timeProvider, ct);

        var userId   = userContext.UserId;
        var email    = userContext.Email ?? request.Email;

        var ticketIds = new List<Guid>();

        foreach (var dto in request.Connections)
        {
            var connection = await connectionRepository.FindByIdAsync(dto.Id, ct)
                             ?? throw new TrackEasyException(SharedCodes.EntityNotFound,
                                 $"Connection {dto.Id} was not found.");

            if (!connection.IsConnectionRunning(dto.ConnectionDate))
                throw new TrackEasyException(SharedCodes.InvalidInput,
                    $"Connection {dto.Id} does not run on {dto.ConnectionDate:yyyy-MM-dd}.");

            var (startStation, endStation) =
                await LoadAndValidateStationsAsync(dto.StartStationId, dto.EndStationId,
                    connection, stationRepository, ct);

            var passengers = await BuildPassengerListAsync(request.Passengers, discountRepository, ct);
            
            var (price, seats) = await ticketPriceService.CalculateAsync(
                connection,
                request.Passengers,
                startStation.Id,
                endStation.Id,
                discountCode,
                dto.ConnectionDate,
                ct);

            var ticketStations = SliceStations(connection, startStation.Id, endStation.Id);

            var ticket = Ticket.Create(
                connection.Id,
                connection.Name,
                ticketStations,
                passengers,
                seats,
                price,
                connection.Operator.Id,
                connection.Operator.Code,
                connection.Operator.Name,
                connection.Train.Name,
                userId,
                email,
                dto.ConnectionDate,
                timeProvider);

            ticketRepository.Add(ticket);
            ticketIds.Add(ticket.Id);
        }

        await ticketRepository.SaveChangesAsync(ct);
        return ticketIds;
    }
    
    private static async Task<List<Person>> BuildPassengerListAsync(
        IEnumerable<PersonDto> people,
        IDiscountRepository discountRepo,
        CancellationToken ct)
    {
        var list = new List<Person>();
        foreach (var person in people)
        {
            var discount = person.DiscountId.HasValue
                ? await discountRepo.GetByIdAsync(person.DiscountId.Value, ct)
                : null;

            if (person.DiscountId.HasValue && discount is null)
                throw new TrackEasyException(SharedCodes.EntityNotFound,
                    $"Discount {person.DiscountId} not found.");

            list.Add(new Person(person.FirstName, person.LastName, person.DateOfBirth, discount?.Name));
        }
        return list;
    }

    private static async Task<DiscountCode?> ValidateDiscountCodeAsync(
        Guid? codeId,
        IDiscountCodeRepository repo,
        TimeProvider tp,
        CancellationToken ct)
    {
        if (codeId is null) return null;

        var code = await repo.GetByIdAsync(codeId.Value, ct)
                   ?? throw new TrackEasyException(SharedCodes.EntityNotFound,
                       $"Discount code {codeId} was not found.");

        if (!code.IsActive(tp))
            throw new TrackEasyException(SharedCodes.InvalidInput,
                "The provided discount code is not active.");

        return code;
    }

    private static async Task<(Station start, Station end)> LoadAndValidateStationsAsync(
        Guid startId, Guid endId, Connection connection,
        IStationRepository repo, CancellationToken ct)
    {
        var start = await repo.GetByIdAsync(startId, ct)
                    ?? throw new TrackEasyException(SharedCodes.EntityNotFound,
                        $"Start station {startId} not found.");
        var end   = await repo.GetByIdAsync(endId, ct)
                    ?? throw new TrackEasyException(SharedCodes.EntityNotFound,
                        $"End station {endId} not found.");

        if (start.Id == end.Id)
            throw new TrackEasyException(SharedCodes.InvalidInput,
                "Start and end stations cannot be the same.");

        if (connection.Stations.All(s => s.StationId != start.Id) ||
            connection.Stations.All(s => s.StationId != end.Id))
            throw new TrackEasyException(SharedCodes.InvalidInput,
                "Start and end stations must belong to the connection.");

        return (start, end);
    }

    private static IEnumerable<TicketConnectionStation> SliceStations(
        Connection connection, Guid startId, Guid endId)
    {
        var startSeq = connection.Stations.First(s => s.StationId == startId).SequenceNumber;
        var endSeq   = connection.Stations.First(s => s.StationId == endId).SequenceNumber;

        return connection.Stations
            .Where(s => s.SequenceNumber >= startSeq && s.SequenceNumber <= endSeq)
            .Select(s => new TicketConnectionStation(
                s.Station.Name, s.ArrivalTime, s.DepartureTime, s.SequenceNumber));
    }
}