namespace Redde.Application.DTOs.Auth
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
