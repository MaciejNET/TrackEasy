using TrackEasy.Domain.Cities;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Cities.CreateCity;

public sealed record CreateCityCommand(string Name, Country Country, IEnumerable<string> FunFacts) : ICommand<Guid>;
