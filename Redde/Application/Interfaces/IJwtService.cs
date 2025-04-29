using Redde.Domain.Entities;

namespace Redde.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);
        (string refreshToken, DateTime expiryTime) GenerateRefreshToken();
    }
}
