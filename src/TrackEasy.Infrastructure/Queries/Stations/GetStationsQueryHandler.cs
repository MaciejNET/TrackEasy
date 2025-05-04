using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetStationsQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetStationsQuery, StationDto>
{
    public Task<StationDto> Handle(GetStationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}