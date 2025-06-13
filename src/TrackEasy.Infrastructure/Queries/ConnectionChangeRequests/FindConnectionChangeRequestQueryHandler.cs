using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Connections.FindConnectionChangeRequest;
using TrackEasy.Application.Connections.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.ConnectionChangeRequests;

internal sealed class FindConnectionChangeRequestQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<FindConnectionChangeRequestQuery, ConnectionChangeRequestDetailsDto?>
{
    public async Task<ConnectionChangeRequestDetailsDto?> Handle(FindConnectionChangeRequestQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Connections
            .AsNoTracking()
            .Where(x => x.Id == request.ConnectionId && x.Request != null)
            .Select(x => new ConnectionChangeRequestDetailsDto(
                x.Id,
                x.Name,
                x.Operator.Name,
                x.Request!.RequestType,
                x.Request.Schedule != null ?
                    new ScheduleDto(x.Request.Schedule.ValidFrom, x.Request.Schedule.ValidTo, x.Request.Schedule.DaysOfWeek.ToList()) : null,
                x.Request.Stations != null ?
                    x.Request.Stations.OrderBy(s => s.SequenceNumber)
                        .Select(s => new ConnectionStationDto(s.StationId, s.ArrivalTime, s.DepartureTime, s.SequenceNumber)) : null
            ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
