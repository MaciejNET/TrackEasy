namespace TrackEasy.Application.Tickets.BuyTicket;

public sealed record PersonDto(
    Guid? UserId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    Guid? DiscountId
    );