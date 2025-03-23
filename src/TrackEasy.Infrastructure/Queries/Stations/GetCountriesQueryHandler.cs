using System.ComponentModel;
using System.Reflection;
using TrackEasy.Application.Stations.GetCountries;
using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Infrastructure.Queries.Stations;

internal sealed class GetCountriesQueryHandler : IQueryHandler<GetCountriesQuery, IEnumerable<CountryDto>>
{
    public Task<IEnumerable<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = Enum.GetValues<Country>()
            .Select(country => new CountryDto((int)country, GetEnumDescription(country)))
            .ToList();
        
        return Task.FromResult<IEnumerable<CountryDto>>(countries);
    }
    
    private static string GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field!.GetCustomAttribute<DescriptionAttribute>();
        return attribute == null ? value.ToString() : attribute.Description;
    }
}