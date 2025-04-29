namespace Redde.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
