using TrackEasy.Application.Cities.GetCountries;
using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Infrastructure;

namespace TrackEasy.Infrastructure.Queries.Cities;

internal sealed class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, IEnumerable<CountryDto>>
{
    public Task<IEnumerable<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = Enum.GetValues<Country>()
            .Select(country => new CountryDto(country.GetEnumId(), country.GetEnumDescription()))
            .ToList();
        
        return Task.FromResult<IEnumerable<CountryDto>>(countries);
    }
}