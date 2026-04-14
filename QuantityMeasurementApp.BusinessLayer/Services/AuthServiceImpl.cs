using Microsoft.Extensions.Logging;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Cache;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IAuthRepository          _authRepo;
        private readonly JwtService               _jwtService;
        private readonly RedisService             _redis;
        private readonly ILogger<AuthServiceImpl> _logger;

        public AuthServiceImpl(
            IAuthRepository          authRepo,
            JwtService               jwtService,
            RedisService             redis,
            ILogger<AuthServiceImpl> logger)
        {
            _authRepo   = authRepo;
            _jwtService = jwtService;
            _redis      = redis;
            _logger     = logger;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            _logger.LogInformation("Register attempt for email: {Email}", request.Email);

            if (await _authRepo.EmailExistsAsync(request.Email))
            {
                _logger.LogWarning("Register failed - email already exists: {Email}", request.Email);
                throw new System.InvalidOperationException("Email is already registered.");
            }

            if (await _authRepo.UsernameExistsAsync(request.Name))
            {
                _logger.LogWarning("Register failed - username already taken: {Name}", request.Name);
                throw new System.InvalidOperationException("Username is already taken.");
            }

            var user = new UserEntity
            {
                Username     = request.Name.Trim(),
                Email        = request.Email.Trim().ToLower(),
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role         = request.Role?.Equals("Admin", StringComparison.OrdinalIgnoreCase) == true ? "Admin" : "User"
            };

            await _authRepo.AddAsync(user);
            await _authRepo.SaveChangesAsync();

            _logger.LogInformation("User registered successfully: {Email} Role: {Role}",
                user.Email, user.Role);

            return new AuthResponseDTO
            {
                Name    = user.Username,
                Email   = user.Email,
                Role    = user.Role,
                Message = "Registration successful. Please login to get your token."
            };
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Email);

            var user = await _authRepo.GetByEmailAsync(request.Email.Trim().ToLower())
                       ?? throw new System.UnauthorizedAccessException("Invalid email or password.");

            if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed - wrong password for: {Email}", request.Email);
                throw new System.UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Login failed - account deactivated: {Email}", request.Email);
                throw new System.UnauthorizedAccessException("Account is deactivated.");
            }

            var accessToken  = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();

            _logger.LogInformation("Login successful for: {Email}", user.Email);

            return new AuthResponseDTO
            {
                AccessToken       = accessToken,
                RefreshToken      = refreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
                Name              = user.Username,
                Email             = user.Email,
                Role              = user.Role,
                Message           = "Login successful."
            };
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            _logger.LogInformation("Refresh token attempt");

            if (await _redis.IsTokenBlacklistedAsync(request.AccessToken))
            {
                _logger.LogWarning("Refresh failed - token is blacklisted");
                throw new System.UnauthorizedAccessException("Token has been revoked.");
            }

            var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken)
                            ?? throw new System.UnauthorizedAccessException("Invalid access token.");

            var email = principal.Claims
                .FirstOrDefault(c =>
                    c.Type == System.Security.Claims.ClaimTypes.Email ||
                    c.Type == "email")?.Value
                ?? throw new System.UnauthorizedAccessException("Invalid token claims.");

            var user = await _authRepo.GetByEmailAsync(email)
                       ?? throw new System.UnauthorizedAccessException("User not found.");

            if (user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh failed - invalid or expired refresh token for: {Email}", email);
                throw new System.UnauthorizedAccessException("Refresh token is invalid or expired.");
            }

            var newAccessToken  = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();

            _logger.LogInformation("Token refreshed successfully for: {Email}", email);

            return new AuthResponseDTO
            {
                AccessToken       = newAccessToken,
                RefreshToken      = newRefreshToken,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
                Name              = user.Username,
                Email             = user.Email,
                Role              = user.Role,
                Message           = "Token refreshed successfully."
            };
        }

        public async Task RevokeAsync(string email, string? accessToken = null)
        {
            _logger.LogInformation("Revoke token for: {Email}", email);

            var user = await _authRepo.GetByEmailAsync(email.Trim().ToLower())
                       ?? throw new System.InvalidOperationException("User not found.");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                await _redis.BlacklistTokenAsync(accessToken, TimeSpan.FromMinutes(60));
                _logger.LogInformation("Access token blacklisted for: {Email}", email);
            }

            user.RefreshToken       = null;
            user.RefreshTokenExpiry = null;

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();

            _logger.LogInformation("Revoke successful for: {Email}", email);
        }

        public Task RevokeAsync(string email) => RevokeAsync(email, null);
    }
}