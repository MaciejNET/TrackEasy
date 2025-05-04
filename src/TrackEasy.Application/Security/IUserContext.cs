namespace TrackEasy.Application.Security;

public interface IUserContext
{
    Guid? UserId { get; }
    string? Email { get; }
    string? Role { get; }
    bool? IsTwoFactorVerified { get; }
    Guid? OperatorId { get; }
    bool IsAuthenticated { get; }
}