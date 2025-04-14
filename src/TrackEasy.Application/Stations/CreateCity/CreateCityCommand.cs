using TrackEasy.Domain.Stations;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Stations.CreateCity;

public sealed record CreateCityCommand(string Name, Country Country) : ICommand;
