using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Discounts.UpdateDiscount;

public sealed record UpdateDiscountCommand(Guid Id, string Name, int Percentage) : ICommand;