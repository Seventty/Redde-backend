using Redde.Domain.Entities;
using System.Security.Claims;

namespace Redde.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        (string refreshToken, DateTime expiryTime) GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
