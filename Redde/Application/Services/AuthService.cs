﻿using Google.Apis.Auth;
using Redde.Application.DTOs.Auth;
using Redde.Application.Interfaces;
using Redde.Domain.Entities;
using System.Security.Claims;
using System.Text.Json;

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
                RoleId = 2,
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

        public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if(string.IsNullOrWhiteSpace(request.Token) || string.IsNullOrWhiteSpace(request.RefreshToken))
                throw new Exception("Invalid token or refresh token");
            
            var principal = _jwtService.GetPrincipalFromExpiredToken(request.Token) ?? throw new Exception("Invalid token");

            var email = (principal.FindFirst(ClaimTypes.Email)?.Value) ?? throw new Exception("Invalid token payload");

            var user = await _unitOfWork.Users.FindAsync(
                u => u.Email.ToLower() == email.ToLower(),
                u => u.Role
            );

            if (user == null || user.RefreshToken != request.RefreshToken)
               throw new Exception("Invalid token or refresh token");

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Refresh token expired");

            var newAccessToken = _jwtService.GenerateAccessToken(user);
            var (newRefreshToken, newRefreshTokenExpiryTime) = _jwtService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = newRefreshTokenExpiryTime;
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                Token = newAccessToken,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60")
                ),
                Email = user.Email,
                Name = user.Name
            };
        }

        public async Task<AuthResponse> LoginWithGoogleAsync(OAuthRequest request)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken);

            if (payload == null)
                throw new Exception("Invalid token");

            var user = await _unitOfWork.Users.FindAsync(
                u => u.Provider == "google" && u.ProviderId == payload.Subject,
                u => u.Role
            );

            if (user == null)
            {
                user = new User
                {
                    Name = payload.Name ?? payload.Email.Split('@')[0],
                    Email = payload.Email,
                    Provider = "google",
                    ProviderId = payload.Subject,
                    RoleId = 1
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                user = await _unitOfWork.Users.FindAsync(u => u.Id == user.Id, u => u.Role);
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

        public async Task<AuthResponse> LoginWithGitHubAsync(OauthCodeRequest request)
        {
            var clientId = Environment.GetEnvironmentVariable("GITHUB_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("GITHUB_CLIENT_SECRET");

            using var httpClient = new HttpClient();

            var tokenResponse = await httpClient.PostAsync(
                "https://github.com/login/oauth/access_token",
                new FormUrlEncodedContent(new Dictionary<string, string>
                {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", request.Code }
                })
            );

            var tokenContent = await tokenResponse.Content.ReadAsStringAsync();

            if (!tokenResponse.IsSuccessStatusCode)
                throw new Exception("Error al obtener access_token de GitHub");

            var parsed = System.Web.HttpUtility.ParseQueryString(tokenContent);
            var accessToken = parsed["access_token"];

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("ReddeApp");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var userResponse = await httpClient.GetAsync("https://api.github.com/user");
            var userJson = await userResponse.Content.ReadAsStringAsync();

            var githubUser = JsonSerializer.Deserialize<GithubUser>(userJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (githubUser is null)
                throw new Exception("Error obteniendo usuario desde GitHub");

            var user = await _unitOfWork.Users.FindAsync(
                u => u.Provider == "github" && u.ProviderId == githubUser.Id.ToString(),
                u => u.Role
            );

            if (user == null)
            {
                user = new User
                {
                    Name = githubUser.Name ?? githubUser.Login,
                    Email = githubUser.Email ?? $"{githubUser.Login}@github.com", // fallback
                    Provider = "github",
                    ProviderId = githubUser.Id.ToString(),
                    RoleId = 2
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                user = await _unitOfWork.Users.FindAsync(u => u.Id == user.Id, u => u.Role);
            }

            var jwt = _jwtService.GenerateAccessToken(user);
            var (refreshToken, refreshTokenExpiryTime) = _jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                Token = jwt,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.Parse(Environment.GetEnvironmentVariable("JWT_EXPIRES_MINUTES") ?? "60")
                ),
                Email = user.Email,
                Name = user.Name
            };
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
