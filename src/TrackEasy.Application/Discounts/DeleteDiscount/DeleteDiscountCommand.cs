using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Discounts.DeleteDiscount;

public sealed record DeleteDiscountCommand(Guid Id) : ICommand;