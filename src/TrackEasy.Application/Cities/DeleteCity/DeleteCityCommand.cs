using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Cities.DeleteCity;

public sealed record DeleteCityCommand(Guid Id) : ICommand;
