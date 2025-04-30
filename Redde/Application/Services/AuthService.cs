using Redde.Application.DTOs.Auth;
using Redde.Application.Interfaces;
using Redde.Domain.Entities;

namespace Redde.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork, IJwtService jwtService) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _unitOfWork.Users.FindAsync(
                u => u.Email.ToLower() == request.Email.ToLower()
            );

            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }

            var user = new User
            {
                Email = request.Email,
                Password = HashPassword(request.Password),
                Name = request.Name,
                RoleId = 1,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var registeredUser = await _unitOfWork.Users.FindAsync(
                u => u.Email.ToLower() == request.Email.ToLower(),
                u => u.Role
            );

            if (registeredUser == null)
            {
                throw new Exception("User registration failed unexpectedly.");
            }

            var accessToken = _jwtService.GenerateAccessToken(registeredUser);
            var (refreshToken, refreshTokenExpiryTime) = _jwtService.GenerateRefreshToken();

            registeredUser.RefreshToken = refreshToken;
            registeredUser.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                Token = accessToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60")
                ),
                Email = registeredUser.Email,
                Name = registeredUser.Name
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.FindAsync(
                u => u.Email.ToLower() == request.Email.ToLower(),
                u => u.Role
            );

            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            var (refreshToken, refreshTokenExpiryTime) = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                Token = accessToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60")
                ),
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var user = await _unitOfWork.Users.FindAsync(
                u => u.Email.ToLower() == request.Email.ToLower()
            );

            if (user == null)
            {
                throw new Exception("User not found");
            }

        }

        public async Task LogoutAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _unitOfWork.SaveChangesAsync();
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
