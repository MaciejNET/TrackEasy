using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.GetCities;

public sealed record GetCitiesQuery() : IQuery<IReadOnlyList<CityDto>>;
