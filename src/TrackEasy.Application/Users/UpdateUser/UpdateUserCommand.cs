using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, string FirstName, string LastName, DateOnly BirthDate) : ICommand;