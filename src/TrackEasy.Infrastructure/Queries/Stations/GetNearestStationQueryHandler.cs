using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations;
using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Domain.Stations;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetNearestStationQueryHandler(TrackEasyDbContext dbContext) : IQueryHandler<GetNearestStationQuery, StationDto?>
{
    public async Task<StationDto?> Handle(GetNearestStationQuery request, CancellationToken cancellationToken)
    {
        var stations = await dbContext.Stations
            .Include(s => s.City)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var requestCoords = new GeographicalCoordinates(
            request.GeographicalCoordinates.Latitude,
            request.GeographicalCoordinates.Longitude);

        var nearest = stations
            .OrderBy(s => s.GeographicalCoordinates.CalculateDistanceTo(requestCoords))
            .FirstOrDefault();

        return nearest is null
            ? null
            : new StationDto(nearest.Id, nearest.Name, nearest.City.Name);
    }
}