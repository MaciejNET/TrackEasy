namespace TrackEasy.Application.Security;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string role, bool twoFactorVerified, Guid? operatorId = null);
}