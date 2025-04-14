using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.GetCities;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetCitiesQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<GetCitiesQuery, IReadOnlyList<CityDto>>
{
    public async Task<IReadOnlyList<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Select(x => new CityDto(x.Id, x.Name, x.Country))
            .ToListAsync(cancellationToken);
    }
}
