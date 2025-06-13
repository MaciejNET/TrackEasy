using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Connections.GetConnectionChangeRequests;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.ConnectionChangeRequests;

internal sealed class GetConnectionChangeRequestsQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetConnectionChangeRequestsQuery, PaginatedResult<ConnectionChangeRequestDto>>
{
    public async Task<PaginatedResult<ConnectionChangeRequestDto>> Handle(GetConnectionChangeRequestsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Connections
            .AsNoTracking()
            .Where(x => x.Request != null)
            .OrderBy(x => x.Name)
            .Select(x => new ConnectionChangeRequestDto(
                x.Id,
                x.Name,
                x.Operator.Name,
                x.Stations.OrderBy(s => s.SequenceNumber).First().Station.Name,
                x.Stations.OrderBy(s => s.SequenceNumber).Last().Station.Name,
                x.Request!.RequestType
            ));

        return await query.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
