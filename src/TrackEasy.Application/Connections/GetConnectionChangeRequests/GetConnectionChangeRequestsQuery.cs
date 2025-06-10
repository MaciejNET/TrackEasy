using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Connections.GetConnectionChangeRequests;

public sealed record GetConnectionChangeRequestsQuery(int PageNumber, int PageSize)
    : IQuery<PaginatedResult<ConnectionChangeRequestDto>>;
