using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Discounts.CreateDiscount;

public sealed record CreateDiscountCommand(string Name, int Percentage) : ICommand<Guid>;