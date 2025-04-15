using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.DeleteCity;

public sealed record DeleteCityCommand(Guid Id) : ICommand;
