using TrackEasy.Application.Stations.Shared;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Stations.GetCities;

public sealed record GetCitiesQuery(int PageNumber, int PageSize) : IQuery<PaginatedResult<CityDto>>;
