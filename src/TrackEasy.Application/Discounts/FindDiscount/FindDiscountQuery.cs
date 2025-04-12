using TrackEasy.Application.Discounts.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Discounts.FindDiscount;

public sealed record FindDiscountQuery(Guid Id) : IQuery<DiscountDto?>;