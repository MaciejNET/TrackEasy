using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.CreateStation;

public sealed record CreateStationCommand(string Name, Guid CityId, GeographicalCoordinatesDto GeographicalCoordinates) : ICommand<Guid>;