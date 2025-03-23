namespace TrackEasy.Shared.Pagination.Abstractions;

public sealed record PaginatedCursorResult<T>(
    IEnumerable<T> Items,
    string? NextCursor,
    bool HasNextPage);