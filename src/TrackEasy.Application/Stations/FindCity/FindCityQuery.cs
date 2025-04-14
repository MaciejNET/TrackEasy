using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.FindCity;

public sealed record FindCityQuery(Guid Id) : IQuery<CityDto?>;
