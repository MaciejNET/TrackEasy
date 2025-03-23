using TrackEasy.Application.DiscountCodes.Shared;
using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.DiscountCodes.FindDiscountCode;

public sealed record FindDiscountCodeQuery(string Code) : IQuery<DiscountCodeDto?>;