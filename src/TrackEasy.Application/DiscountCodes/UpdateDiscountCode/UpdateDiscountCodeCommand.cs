using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.DiscountCodes.UpdateDiscountCode;

public sealed record UpdateDiscountCodeCommand(Guid Id, int Percentage, DateTime From, DateTime To) : ICommand;