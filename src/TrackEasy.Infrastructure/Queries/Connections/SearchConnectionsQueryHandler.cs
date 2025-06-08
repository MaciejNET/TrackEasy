using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Connections.SearchConnections;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Connections;

internal sealed class SearchConnectionsQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<SearchConnectionsQuery, PaginatedCursorResult<SearchConnectionsResponse>>
{
    private const int PageSize = 10;          // hard-coded “TOP 10”

    public async Task<PaginatedCursorResult<SearchConnectionsResponse>> Handle(
        SearchConnectionsQuery request,
        CancellationToken      cancellationToken)
    {
        // --------------------------------------------------------------------
        // 1.  Cursor = DepartureTime of the request
        // --------------------------------------------------------------------
        DateTime arrivalCursor = request.DepartureTime;

        // --------------------------------------------------------------------
        // 2.  SEARCH – Dijkstra-like, lazy expansion
        // --------------------------------------------------------------------
        var heap = new PriorityQueue<PathCandidate, (DateTime, Guid)>();

        // Seed the queue with every first leg that leaves the start station
        await foreach (var firstLeg in OutgoingLegsAsync(
                           request.StartStationId,
                           request.DepartureTime,
                           cancellationToken))
        {
            var p = new PathCandidate(firstLeg);
            heap.Enqueue(p, PriorityKey(p));
        }

        var page   = new List<SearchConnectionsResponse>(PageSize);
        DateTime   lastArrival = arrivalCursor;

        while (heap.Count > 0 && page.Count < PageSize)
        {
            var path = heap.Dequeue();

            // Resume after last cursor
            if (IsBeforeCursor(path, arrivalCursor))
                continue;

            // Complete?
            if (path.LastLeg.ToStationId == request.EndStationId)
            {
                page.Add(ToResponse(path));
                lastArrival = path.LastLeg.ArrivalDateTime;
                continue;
            }

            // Expand one more transfer
            await foreach (var next in OutgoingLegsAsync(
                               path.LastLeg.ToStationId,
                               path.LastLeg.ArrivalDateTime.AddMinutes(1),
                               cancellationToken))
            {
                if (path.ContainsStation(next.ToStationId))
                    continue;       // avoid loops

                var longer = path.Append(next);
                heap.Enqueue(longer, PriorityKey(longer));
            }
        }

        // --------------------------------------------------------------------
        // 3.  Build cursor for the “next” page
        // --------------------------------------------------------------------
        var nextCursor = heap.Count == 0
            ? null
            : lastArrival.ToString("O"); // ISO‑8601 timestamp

        return new PaginatedCursorResult<SearchConnectionsResponse>(
            Items:       page,
            NextCursor:  nextCursor,
            HasNextPage: nextCursor is not null);
    }

    // ========================================================================
    // >>>                   Helpers & private types                        <<<
    // ========================================================================

    #region Cursor helpers ----------------------------------------------------

    private static (DateTime, Guid) PriorityKey(PathCandidate p)
        => (p.LastLeg.ArrivalDateTime, p.Hash);

    private static bool IsBeforeCursor(PathCandidate path, DateTime arrivalCur)
        => path.LastLeg.ArrivalDateTime <= arrivalCur;

    #endregion

    #region Path / leg domain -------------------------------------------------

    private sealed record Leg(
        Guid       LegId,
        Guid       ConnectionId,
        string     ConnectionName,
        string     OperatorName,
        string     OperatorCode,
        Guid       FromStationId,
        string     FromStationName,
        Guid       ToStationId,
        string     ToStationName,
        DateTime   DepartureDateTime,
        DateTime   ArrivalDateTime,
        decimal    PricePerKilometre);

    private sealed class PathCandidate
    {
        private readonly List<Leg> _legs;
        public          Guid       Hash  { get; }  = Guid.NewGuid();
        public          Leg        LastLeg => _legs[^1];

        public PathCandidate(Leg first) => _legs = [ first ];

        private PathCandidate(List<Leg> copy, Leg append)
        { _legs = copy; _legs.Add(append); }

        public PathCandidate Append(Leg next) => new([.._legs], next);

        public bool ContainsStation(Guid stationId)
            => _legs.Any(l => l.FromStationId == stationId ||
                              l.ToStationId   == stationId);

        public IReadOnlyList<Leg> Legs => _legs.AsReadOnly();
    }

    #endregion

    #region EF – outgoing-legs generator -------------------------------------

    private async IAsyncEnumerable<Leg> OutgoingLegsAsync(
        Guid               stationId,
        DateTime           notBeforeUtc,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        // Day & time parts used for filtering
        var date        = DateOnly.FromDateTime(notBeforeUtc);
        var time        = TimeOnly.FromDateTime(notBeforeUtc);
        var dayOfWeek   = notBeforeUtc.DayOfWeek;

        // 1. Query every *activated* connection that
        //    • is valid on `date` and
        //    • runs on this `dayOfWeek` and
        //    • has a station matching `stationId`
        //    • whose departure time is >= `time`
        //
        // NB: EF Core 8 still can’t translate TimeOnly well, so we split
        //     the query – SQL for coarse filtering, LINQ to Objects for fine.
        await foreach (var conn in dbContext.Connections
            .AsNoTracking()
            .Include(x => x.Stations)
                .ThenInclude(x => x.Station)
            .Include(x => x.Operator)
            .Include(x => x.Schedule)
            .Where(c => c.IsActivated &&
                        c.Schedule.ValidFrom <= date &&
                        c.Schedule.ValidTo   >= date)
            .AsAsyncEnumerable()
            .WithCancellation(ct))
        {
            if (conn.Schedule.DaysOfWeek.Count != 0 &&
                !conn.Schedule.DaysOfWeek.Contains(dayOfWeek))
            {
                continue;   // wrong day
            }

            // Pull the ordered list *once*
            var ordered = conn.Stations
                              .OrderBy(s => s.SequenceNumber)
                              .ToArray();

            // Locate the boarding station
            for (var i = 0; i < ordered.Length - 1; i++)
            {
                var from = ordered[i];
                var to   = ordered[i + 1];

                if (from.StationId != stationId) continue;
                if (from.DepartureTime is null ||
                    from.DepartureTime < time)  continue; // too early
                if (to.ArrivalTime is null)     continue;

                yield return new Leg(
                    LegId:             Guid.NewGuid(),
                    ConnectionId:      conn.Id,
                    ConnectionName:    conn.Name,
                    OperatorName:      conn.Operator.Name,
                    OperatorCode:      conn.Operator.Code,
                    FromStationId:     from.StationId,
                    FromStationName:   from.Station.Name,
                    ToStationId:       to.StationId,
                    ToStationName:     to.Station.Name,
                    DepartureDateTime: Combine(date, from.DepartureTime.Value),
                    ArrivalDateTime:   Combine(date, to.ArrivalTime.Value),
                    PricePerKilometre: conn.PricePerKilometer.Amount);
            }
        }
    }

    private static DateTime Combine(DateOnly d, TimeOnly t)
        => d.ToDateTime(t, DateTimeKind.Unspecified);

    #endregion

    #region Mapping to DTO ----------------------------------------------------

    private static SearchConnectionsResponse ToResponse(PathCandidate path)
    {
        var legs = path.Legs;

        var connDtos = legs.Select(l => new ConnectionDto(
                l.ConnectionId,
                l.ConnectionName,
                l.OperatorName,
                l.OperatorCode,
                TimeOnly.FromDateTime(l.DepartureDateTime),
                TimeOnly.FromDateTime(l.ArrivalDateTime),
                l.FromStationId,
                l.FromStationName,
                l.ToStationId,
                l.ToStationName,
                Price: l.PricePerKilometre))      // simple: km-price only
            .ToList();

        var first  = legs[0];
        var last   = legs[^1];

        return new SearchConnectionsResponse(
            Connections:    connDtos,
            TransfersCount: connDtos.Count - 1,
            StartStation:   first.FromStationName,
            EndStation:     last.ToStationName,
            DepartureTime:  TimeOnly.FromDateTime(first.DepartureDateTime),
            ArrivalTime:    TimeOnly.FromDateTime(last.ArrivalDateTime));
    }

    #endregion
}