using Redde.Application.DTOs.Auth;
using Redde.Application.Interfaces;
using Redde.Domain.Entities;

namespace Redde.Application.Services
{
    public class AuthService(IUnitOfWork unitOfWork) : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _unitOfWork.Users.GetAllAsync();

            if (existingUser.Any(u => u.Email.ToLower() == request.Email.ToLower()))
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

            return new AuthResponse
            {
                Token = "To_Be_generated",
                Expiration = DateTime.UtcNow.AddHours(1),
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());

            if (user == null || !VerifyPassword(request.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }

            return new AuthResponse
            {
                Token = "TO_BE_GENERATED",
                Expiration = DateTime.UtcNow.AddHours(1),
                Email = user.Email,
                Name = user.Name
            };
        }
        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var user = users.FirstOrDefault(u => u.Email.ToLower() == request.Email.ToLower());
            if (user == null)
            {
                throw new Exception("User not found");
            }
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
