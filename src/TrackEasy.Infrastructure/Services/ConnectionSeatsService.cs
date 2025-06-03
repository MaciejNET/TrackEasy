using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Services;
using TrackEasy.Infrastructure.Database;

namespace TrackEasy.Infrastructure.Services;

internal sealed class ConnectionSeatsService(TrackEasyDbContext dbContext) : IConnectionSeatsService
{
    public async Task<List<int>?> GetAvailableSeatsAsync(Guid connectionId, DateOnly date, Guid startStationId, Guid endStationId,
        CancellationToken cancellationToken)
    {
       var connection = await dbContext.Connections
        .Include(c => c.Stations)
        .Include(c => c.Train)
            .ThenInclude(t => t.Coaches)
                .ThenInclude(tc => tc.Coach)
                    .ThenInclude(co => co.Seats)
        .FirstOrDefaultAsync(c => c.Id == connectionId, cancellationToken);

    if (connection is null)
        return null;

    if (!connection.NeedsSeatReservation)
        return null;

    var startSeq = connection.Stations.First(s => s.StationId == startStationId).SequenceNumber;
    var endSeq   = connection.Stations.First(s => s.StationId == endStationId).SequenceNumber;

    if (startSeq > endSeq) (startSeq, endSeq) = (endSeq, startSeq);

    var allSeatNumbers = connection.Train.Coaches
        .SelectMany(c => c.Coach.Seats)
        .Select(s => s.Number)
        .Distinct()
        .ToList();

    var takenSeatNumbers = await dbContext.Tickets
        .Where(t => t.ConnectionId   == connectionId &&
                    t.ConnectionDate == date        &&
                    t.SeatNumbers    != null        &&
                    t.Stations.Any(st =>
                        st.SequenceNumber >= startSeq &&
                        st.SequenceNumber <= endSeq))
        .SelectMany(t => t.SeatNumbers!)
        .Distinct()
        .ToListAsync(cancellationToken);

    var freeSeats = allSeatNumbers
        .Except(takenSeatNumbers)
        .OrderBy(n => n)
        .ToList();

    return freeSeats;   
                    
    }
}