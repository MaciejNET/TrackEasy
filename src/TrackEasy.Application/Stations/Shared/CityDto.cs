using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Shared;

public sealed record CityDto(Guid Id, string Name, Country Country);
