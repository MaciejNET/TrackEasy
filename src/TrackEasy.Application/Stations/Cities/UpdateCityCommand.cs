using MediatR;
using System;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities.Commands;

public record UpdateCityCommand(Guid Id, string Name, CityEnum CityEnum) : IRequest<bool>;
