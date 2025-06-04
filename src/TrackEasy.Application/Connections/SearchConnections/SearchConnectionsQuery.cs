using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Connections.SearchConnections;

public sealed record SearchConnectionsQuery(Guid StartStationId, Guid EndStationId, DateTime DepartureTime) : IQuery<PaginatedCursorResult<SearchConnectionsResponse>>;