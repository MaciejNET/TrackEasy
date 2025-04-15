using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.GetCities;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetCitiesQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetCitiesQuery, PaginatedResult<CityDto>>
{
    public async Task<PaginatedResult<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Select(x => new CityDto(x.Id, x.Name, x.Country))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
