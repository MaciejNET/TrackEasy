using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword) : ICommand;