using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.RefundRequests.GetRefundRequests;

public sealed record GetRefundRequestsQuery(Guid OperatorId, int PageNumber, int PageSize)
    : IQuery<PaginatedResult<RefundRequestDto>>;
