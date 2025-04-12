using TrackEasy.Application.Discounts.Shared;
using TrackEasy.Shared.Application.Abstractions;
using TrackEasy.Shared.Pagination.Abstractions;

namespace TrackEasy.Application.Discounts.GetDiscounts;

public sealed record GetDiscountsQuery(int PageNumber, int PageSize) 
    : IQuery<PaginatedResult<DiscountDto>>;