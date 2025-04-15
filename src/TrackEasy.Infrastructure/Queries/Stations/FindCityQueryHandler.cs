using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Stations.FindCity;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class FindCityQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<FindCityQuery, CityDto?>
{
    public async Task<CityDto?> Handle(FindCityQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new CityDto(x.Id, x.Name, x.Country))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
