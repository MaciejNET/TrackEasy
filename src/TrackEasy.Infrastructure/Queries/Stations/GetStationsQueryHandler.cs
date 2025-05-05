using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetStationsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetStationsQuery, PaginatedResult<StationDto>>
{
    public Task<PaginatedResult<StationDto>> Handle(GetStationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}