using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.ConfirmEmail;

public sealed record ConfirmEmailCommand(string Email, string Token) : ICommand;