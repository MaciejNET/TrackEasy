using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : ICommand;