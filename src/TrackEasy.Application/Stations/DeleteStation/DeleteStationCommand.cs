using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.DeleteStation;

public sealed record DeleteStationCommand(Guid Id) : ICommand;