using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.ExternalLogin;

public sealed record ExternalLoginCommand(string Provider, string FirstName, string LastName, DateOnly DateOfBirth) : ICommand<string>;
