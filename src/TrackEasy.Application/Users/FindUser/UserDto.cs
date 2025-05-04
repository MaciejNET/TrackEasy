namespace TrackEasy.Application.Users.FindUser;

public sealed record UserDto(Guid Id, string FirstName, string LastName, string Email, string Role, Guid? OperatorId);