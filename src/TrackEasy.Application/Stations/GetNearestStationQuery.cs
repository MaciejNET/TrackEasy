using TrackEasy.Application.Stations.GetStations;
using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations;

public sealed record GetNearestStationQuery(GeographicalCoordinatesDto GeographicalCoordinates) : IQuery<StationDto?>;