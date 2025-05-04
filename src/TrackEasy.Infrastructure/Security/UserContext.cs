using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TrackEasy.Application.Security;

namespace TrackEasy.Infrastructure.Security;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId => GetOptionalGuidClaim(ClaimTypes.NameIdentifier);
    public string? Email => GetOptionalClaimValue(ClaimTypes.Email);
    public string? Role => GetOptionalClaimValue(ClaimTypes.Role);
    public bool? IsTwoFactorVerified => GetOptionalBooleanClaim(CustomClaims.TwoFactorVerified);
    public Guid? OperatorId => GetOptionalGuidClaim(CustomClaims.OperatorId);
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    
    private string? GetOptionalClaimValue(string claimType)
    {
        return httpContextAccessor.HttpContext?.User?
            .FindFirst(claimType)?.Value;
    }

    private Guid? GetOptionalGuidClaim(string claimType)
    {
        var value = GetOptionalClaimValue(claimType);
        return Guid.TryParse(value, out var guid) ? guid : null;
    }

    private bool? GetOptionalBooleanClaim(string claimType)
    {
        var value = GetOptionalClaimValue(claimType);
        return bool.TryParse(value, out var result) ? result : null;
    }
}