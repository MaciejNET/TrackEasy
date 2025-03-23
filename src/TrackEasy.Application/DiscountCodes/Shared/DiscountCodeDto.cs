namespace TrackEasy.Application.DiscountCodes.Shared;

public record DiscountCodeDto(Guid Id, string Code, decimal Percentage, DateTime From, DateTime To);