using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.UpdateStation;

public sealed record UpdateStationCommand(Guid Id, string Name, GeographicalCoordinatesDto GeographicalCoordinates) : ICommand;