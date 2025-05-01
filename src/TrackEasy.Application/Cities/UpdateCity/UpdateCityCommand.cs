using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Cities.UpdateCity;

public sealed record UpdateCityCommand(Guid Id, string Name, Country Country, IEnumerable<string> FunFacts) : ICommand;
