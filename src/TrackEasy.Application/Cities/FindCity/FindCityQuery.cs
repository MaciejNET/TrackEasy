using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Cities.FindCity;

public sealed record FindCityQuery(Guid Id) : IQuery<CityDetailsDto?>;
