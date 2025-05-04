using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.GenerateToken;

public sealed record GenerateTokenCommand(string Email, string Password) : ICommand<string>;