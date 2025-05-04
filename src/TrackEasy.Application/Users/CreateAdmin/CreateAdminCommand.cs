using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.CreateAdmin;

public sealed record CreateAdminCommand(string FirstName, string LastName, string Email, DateOnly DateOfBirth, string Password) : ICommand;