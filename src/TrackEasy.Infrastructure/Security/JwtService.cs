using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TrackEasy.Application.Security;

namespace TrackEasy.Infrastructure.Security;

internal sealed class JwtService(IOptions<JwtSettings> jwtSettings) : IJwtService
{
    public string GenerateToken(Guid userId, string email, string role, bool twoFactorVerified, Guid? operatorId = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Value.Key);
        
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Role, role)
        ];
        
        if (operatorId.HasValue)
        {
            claims.Add(new Claim(CustomClaims.OperatorId, operatorId.Value.ToString()));
        }

        if (twoFactorVerified)
        {
            claims.Add(new Claim(CustomClaims.TwoFactorVerified, "true"));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Value.ExpirationInMinutes),
            Issuer = jwtSettings.Value.Issuer,
            Audience = jwtSettings.Value.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}