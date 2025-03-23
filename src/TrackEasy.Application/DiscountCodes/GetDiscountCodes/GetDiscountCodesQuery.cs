using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.DiscountCodes.GetDiscountCodes;

public sealed record GetDiscountCodesQuery(int PageNumber, int PageSize) : IQuery<PaginatedResult<DiscountCodeDto>>;