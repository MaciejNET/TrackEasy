using TrackEasy.Shared.Application.Abstractions;

namespace TrackEasy.Application.Users.FindUser;

public sealed record FindUserQuery() : IQuery<UserDto>;