using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.GenerateTwoFactorToken;

public sealed record GenerateTwoFactorTokenCommand(string Code) : ICommand<string>;