using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Tickets.GetTicketCities;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class GetTicketCitiesQueryHandler(TrackEasyDbContext dbContext, TimeProvider timeProvider) : IQueryHandler<GetTicketCitiesQuery, IEnumerable<TicketCityDto>>
{
    public async Task<IEnumerable<TicketCityDto>> Handle(GetTicketCitiesQuery request, CancellationToken cancellationToken)
    {
       var ticket = await dbContext.Tickets
            .Where(x => x.Id == request.Id)
            .Include(x => x.Stations)
            .SingleOrDefaultAsync(cancellationToken);
        if (ticket is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Ticket with ID {request.Id} was not found.");
        }

        var connection = await dbContext.Connections
            .Where(x => x.Id == ticket.ConnectionId)
            .Include(x => x.Stations)
                .ThenInclude(cs => cs.Station)
                    .ThenInclude(s => s.City)
            .SingleOrDefaultAsync(cancellationToken);
        if (connection is null)
        {
            throw new TrackEasyException(SharedCodes.EntityNotFound, $"Connection with ID {ticket.ConnectionId} was not found.");
        }

        var stopsWithArrival = connection.Stations
            .Where(cs => ticket.Stations.Any(ts => ts.SequenceNumber == cs.SequenceNumber))
            .Select(cs =>
            {
                var ts = ticket.Stations.Single(ts => ts.SequenceNumber == cs.SequenceNumber);
                var arrivalLocal = ts.ArrivalTime.HasValue
                    ? ticket.ConnectionDate.ToDateTime(ts.ArrivalTime.Value)
                    : (DateTime?)null;
                return new { cs.SequenceNumber, City = cs.Station.City, Arrival = arrivalLocal };
            })
            .Where(x => x.Arrival.HasValue)
            .ToList();

        var now = timeProvider.GetUtcNow().DateTime;
        var windowEnd = now.AddMinutes(5);

        var relevant = stopsWithArrival
            .Where(x => x.Arrival.GetValueOrDefault() <= windowEnd)
            .OrderBy(x => x.SequenceNumber)
            .ToList();

        var lastArrivedSeq = relevant
            .Where(x => x.Arrival.GetValueOrDefault() <= now)
            .Select(x => x.SequenceNumber)
            .DefaultIfEmpty(0)
            .Max();

        return relevant
            .Select(x => new TicketCityDto(
                x.City.Id,
                x.City.Name,
                x.SequenceNumber,
                x.SequenceNumber > lastArrivedSeq))
            .ToList();
    }
}