using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Tickets.GetTicketArrivalTimes;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Exceptions;

namespace TrackEasy.Infrastructure.Queries.Tickets;

internal sealed class GetTicketArrivalTimesQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetTicketArrivalTimesQuery, IEnumerable<TicketArrivalDto>>
{
    public async Task<IEnumerable<TicketArrivalDto>> Handle(GetTicketArrivalTimesQuery request, CancellationToken cancellationToken)
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

        return connection.Stations
            .Where(cs => ticket.Stations.Any(ts => ts.SequenceNumber == cs.SequenceNumber && ts.ArrivalTime.HasValue))
            .OrderBy(cs => cs.SequenceNumber)
            .Select(cs => new TicketArrivalDto(
                cs.Station.City.Name,
                ticket.Stations.Single(ts => ts.SequenceNumber == cs.SequenceNumber).ArrivalTime!.Value))
            .ToList();
    }
}
