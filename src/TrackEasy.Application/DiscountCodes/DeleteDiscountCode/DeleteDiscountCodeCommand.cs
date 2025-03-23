using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.DiscountCodes.DeleteDiscountCode;

public sealed record DeleteDiscountCodeCommand(Guid Id) : ICommand;