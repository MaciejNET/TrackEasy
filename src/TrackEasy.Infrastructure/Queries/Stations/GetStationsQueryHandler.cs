using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetStationsQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetStationsQuery, PaginatedResult<StationDto>>
{
    public async Task<PaginatedResult<StationDto>> Handle(GetStationsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Stations
            .AsNoTracking()
            .Include(s => s.City)
            .WithStationName(request.StationName)
            .WithCityName(request.CityName)
            .OrderBy(s => s.Name)
            .Select(s => new StationDto(s.Id, s.Name, s.City.Name));

        return await query.PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}