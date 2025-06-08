using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Connections.FindConnection;
using TrackEasy.Application.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Connections;

internal sealed class FindConnectionQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindConnectionQuery, ConnectionDetailsDto?>
{
    public async Task<ConnectionDetailsDto?> Handle(FindConnectionQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Connections
            .Where(x => x.Id == request.Id)
            .Select(x => new ConnectionDetailsDto(
                x.Id,
                x.Name,
                new MoneyDto(x.PricePerKilometer.Amount, x.PricePerKilometer.Currency),
                x.Train.Id,
                x.Train.Name,
                x.Schedule.ValidFrom,
                x.Schedule.ValidTo,
                x.Schedule.DaysOfWeek,
                x.Stations.Select(s => new ConnectionStationDetailsDto(
                    s.Id,
                    s.Station.Id,
                    s.Station.Name,
                    s.DepartureTime,
                    s.ArrivalTime,
                    s.SequenceNumber
                )),
                x.Request != null,
                x.NeedsSeatReservation,
                x.IsActivated
            ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}