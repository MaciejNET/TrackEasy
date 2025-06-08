using TrackEasy.Application.Connections.GetConnections;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Connections;

internal sealed class GetConnectionsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetConnectionsQuery, PaginatedResult<ConnectionDto>>
{
    public async Task<PaginatedResult<ConnectionDto>> Handle(GetConnectionsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Connections
            .Where(x => x.Operator.Id == request.OperatorId)
            .Select(x => new ConnectionDto(
                x.Id,
                x.Name,
                x.Stations.Where(s => s.SequenceNumber == 1).Select(s => s.Station.Name).First(),
                x.Stations.Where(s => s.SequenceNumber == x.Stations.Count).Select(s => s.Station.Name).First(),
                x.Schedule.DaysOfWeek,
                x.IsActivated
            ))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}