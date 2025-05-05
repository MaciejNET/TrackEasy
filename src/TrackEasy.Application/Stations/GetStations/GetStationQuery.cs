using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Stations.GetStations;

public sealed record GetStationsQuery(
    string? StationName,
    string? CityName,
    int PageNumber,
    int PageSize
    ) : IQuery<PaginatedResult<StationDto>>;