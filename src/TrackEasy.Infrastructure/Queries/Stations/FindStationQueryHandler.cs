using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class FindStationQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<FindStationQuery, StationDetailsDto?>
{
    public Task<StationDetailsDto> Handle(FindStationQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}