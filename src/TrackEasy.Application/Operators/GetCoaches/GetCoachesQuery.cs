using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Operators.GetCoaches;

public sealed record GetCoachesQuery(Guid OperatorId, string? Code, int PageNumber, int PageSize) : IQuery<PaginatedResult<CoachDto>>;