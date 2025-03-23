using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.DiscountCodes.CreateDiscountCode;

public sealed record CreateDiscountCodeCommand(string Code, int Percentage, DateTime From, DateTime To) : ICommand;