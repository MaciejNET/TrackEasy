using MediatR;
using System;

namespace TrackEasy.Application.Stations.Cities.Commands;

public record DeleteCityCommand(Guid Id) : IRequest<bool>;
