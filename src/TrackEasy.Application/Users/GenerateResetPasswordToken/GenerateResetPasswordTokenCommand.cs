using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.GenerateResetPasswordToken;

public sealed record GenerateResetPasswordTokenCommand(string Email, string Password) : ICommand<string>;