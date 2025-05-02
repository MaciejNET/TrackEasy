using TrackEasy.Application.Cities.GetCountries;

namespace TrackEasy.Application.Cities.FindCity;

public sealed record CityDetailsDto(Guid Id, string Name, CountryDto Country, IEnumerable<string> FunFacts);