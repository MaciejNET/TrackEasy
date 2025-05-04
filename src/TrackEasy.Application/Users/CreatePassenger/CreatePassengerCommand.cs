using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.CreatePassenger;

public sealed record CreatePassengerCommand(string FirstName, string LastName, string Email, string Password, DateOnly DateOfBirth) : ICommand;