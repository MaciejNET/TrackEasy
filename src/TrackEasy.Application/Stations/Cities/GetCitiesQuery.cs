using MediatR;
using System.Collections.Generic;
using TrackEasy.Domain.Stations;

namespace TrackEasy.Application.Stations.Cities.Queries;

public record GetCitiesQuery() : IRequest<IEnumerable<City>>;
