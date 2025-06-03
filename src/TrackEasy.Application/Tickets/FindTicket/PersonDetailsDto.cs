namespace TrackEasy.Application.Tickets.FindTicket;

public sealed record PersonDetailsDto(string FirstName, string LastName, DateOnly DateOfBirth, string? Discount);