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
        return await dbContext.Stations
            .AsNoTracking()
            .Where(st => st.Id == request.Id)
            .Select(st => new StationDetailsDto(
                st.Id,
                st.Name,
                st.CityId,
                st.City.Name,
                new GeographicalCoordinatesDto(
                    st.GeographicalCoordinates.Latitude,
                    st.GeographicalCoordinates.Longitude)))
            .SingleOrDefaultAsync(cancellationToken);
    }
}