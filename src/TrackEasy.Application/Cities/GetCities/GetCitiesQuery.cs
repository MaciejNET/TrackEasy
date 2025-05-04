using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Cities.GetCities;

public sealed record GetCitiesQuery(string? Name, int PageNumber, int PageSize) : IQuery<PaginatedResult<CityDto>>;
