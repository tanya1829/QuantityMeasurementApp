using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly IAuthRepository _authRepo;
        private readonly JwtService      _jwtService;

        public AuthServiceImpl(IAuthRepository authRepo, JwtService jwtService)
        {
            _authRepo   = authRepo;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            if (await _authRepo.EmailExistsAsync(request.Email))
                throw new System.InvalidOperationException(
                    "Email is already registered.");

            if (await _authRepo.UsernameExistsAsync(request.Username))
                throw new System.InvalidOperationException(
                    "Username is already taken.");

            var user = new UserEntity
            {
                Username     = request.Username.Trim(),
                Email        = request.Email.Trim().ToLower(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role         = request.Role == "Admin" ? "Admin" : "User"
            };

            var accessToken  = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _authRepo.AddAsync(user);
            await _authRepo.SaveChangesAsync();

            return BuildResponse(user, accessToken, refreshToken);
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var user = await _authRepo.GetByEmailAsync(
                           request.Email.Trim().ToLower())
                       ?? throw new System.UnauthorizedAccessException(
                           "Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new System.UnauthorizedAccessException(
                    "Invalid email or password.");

            var accessToken  = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();

            return BuildResponse(user, accessToken, refreshToken);
        }

        public async Task<AuthResponseDTO> RefreshTokenAsync(
            RefreshTokenRequestDTO request)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(
                                request.AccessToken)
                            ?? throw new System.UnauthorizedAccessException(
                                "Invalid access token.");

            var email = principal.Claims
                .FirstOrDefault(c =>
                    c.Type == System.Security.Claims.ClaimTypes.Email ||
                    c.Type == "email")?.Value
                ?? throw new System.UnauthorizedAccessException(
                    "Invalid token claims.");

            var user = await _authRepo.GetByEmailAsync(email)
                       ?? throw new System.UnauthorizedAccessException(
                           "User not found.");

            if (user.RefreshToken != request.RefreshToken ||
                user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new System.UnauthorizedAccessException(
                    "Refresh token is invalid or expired.");

            var newAccessToken  = _jwtService.GenerateAccessToken(user);
            var newRefreshToken = _jwtService.GenerateRefreshToken();

            user.RefreshToken       = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();

            return BuildResponse(user, newAccessToken, newRefreshToken);
        }

        public async Task RevokeAsync(string email)
        {
            var user = await _authRepo.GetByEmailAsync(email.Trim().ToLower())
                       ?? throw new System.InvalidOperationException(
                           "User not found.");

            user.RefreshToken       = null;
            user.RefreshTokenExpiry = null;

            await _authRepo.UpdateAsync(user);
            await _authRepo.SaveChangesAsync();
        }

        private static AuthResponseDTO BuildResponse(
            UserEntity user,
            string     accessToken,
            string     refreshToken) => new()
        {
            AccessToken       = accessToken,
            RefreshToken      = refreshToken,
            AccessTokenExpiry = DateTime.UtcNow.AddMinutes(60),
            Username          = user.Username,
            Email             = user.Email,
            Role              = user.Role
        };
    }
}