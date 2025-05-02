using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Cities.GetCountries;

public sealed record GetCountriesQuery : IQuery<IEnumerable<CountryDto>>;

public sealed record CountryDto(int Id, string Name);