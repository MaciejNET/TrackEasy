using MediatR;
using System;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities.Queries;

public record FindCityQuery(Guid Id) : IRequest<City>;
