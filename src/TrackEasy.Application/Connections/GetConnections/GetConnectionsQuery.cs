using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Connections.GetConnections;

public sealed record GetConnectionsQuery(Guid OperatorId, string? Name, string StartStation, string EndStation,
    int PageNumber, int PageSize) : IQuery<PaginatedResult<ConnectionDto>>;