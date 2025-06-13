namespace TrackEasy.Application.Tickets.BuyTicket;

public sealed record PersonDto(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Guid? DiscountId
    );