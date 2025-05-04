using Microsoft.EntityFrameworkCore;
using TrackEasy.Application.Cities.FindCity;
using TrackEasy.Application.Cities.GetCountries;
using TrackEasy.Infrastructure.Database;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Cities;

internal sealed class FindCityQueryHandler(TrackEasyDbContext dbContext)
    : IQueryHandler<FindCityQuery, CityDetailsDto?>
{
    public async Task<CityDetailsDto?> Handle(FindCityQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Cities
            .AsNoTracking()
            .WithCityId(request.Id)
            .Select(x => new CityDetailsDto(
                x.Id,
                x.Name,
                new CountryDto(x.Country.GetEnumId(), x.Country.GetEnumDescription()),
                x.FunFacts.ToList()
                ))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
