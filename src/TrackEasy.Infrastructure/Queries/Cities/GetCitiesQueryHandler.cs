using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Cities.GetCities;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Infrastructure;
using TrackEasy.Shared.Pagination.Abstractions;
using TrackEasy.Shared.Pagination.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Cities;

internal sealed class GetCitiesQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetCitiesQuery, PaginatedResult<CityDto>>
{
    public async Task<PaginatedResult<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Select(x => new CityDto(x.Id, x.Name, x.Country.GetEnumDescription()))
            .PaginateAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}
