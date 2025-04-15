using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.UpdateCity;

public sealed record UpdateCityCommand(Guid Id, string Name, Country Country) : ICommand;
