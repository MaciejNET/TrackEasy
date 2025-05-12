using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.FindStation;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class FindStationQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<FindStationQuery, StationDetailsDto?>
{
    public async Task<StationDetailsDto?> Handle(FindStationQuery request, CancellationToken cancellationToken)
    {
        var station = await dbContext.Stations
            .Include(st => st.City)
            .AsNoTracking()
            .FirstOrDefaultAsync(st => st.Id == request.Id, cancellationToken);

        if (station is null)
            return null;

        var coordsDto = new GeographicalCoordinatesDto(
            station.GeographicalCoordinates.Latitude,
            station.GeographicalCoordinates.Longitude);

        return new StationDetailsDto(
            station.Id,
            station.Name,
            station.City.Name,
            coordsDto);
    }
}