using TrackEasy.Application.Stations.Shared;

namespace TrackEasy.Application.Stations.FindStation;

public sealed record StationDetailsDto(Guid Id, string Name, Guid CityId, string CityName, GeographicalCoordinatesDto GeographicalCoordinates);