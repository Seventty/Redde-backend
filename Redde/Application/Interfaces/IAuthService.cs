using Redde.Application.DTOs.Auth;

namespace Redde.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task ForgotPasswordAsync(ForgotPasswordRequest request);

        Task LogoutAsync(int userId);
    }
}
