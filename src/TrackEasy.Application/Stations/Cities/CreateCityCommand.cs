using MediatR;
using System;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities.Commands;

public record CreateCityCommand(string Name, CityEnum CityEnum) : IRequest<Guid>;
