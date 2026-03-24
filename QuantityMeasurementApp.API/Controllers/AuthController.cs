using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.BusinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;

namespace QuantityMeasurementApp.API.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService          _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger      = logger;
        }

        /// <summary>Register a new user — returns no token, use login to get token</summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            _logger.LogInformation("Register request for: {Email}", request.Email);
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        /// <summary>Login — returns accessToken and refreshToken</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            _logger.LogInformation("Login request for: {Email}", request.Email);
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        /// <summary>Get new access token using refresh token</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
        {
            _logger.LogInformation("Refresh token request");
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }

        /// <summary>Logout — revokes refresh token and blacklists access token</summary>
        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> Revoke()
        {
            var email = User.Claims
                .FirstOrDefault(c =>
                    c.Type == System.Security.Claims.ClaimTypes.Email ||
                    c.Type == "email")?.Value;

            if (string.IsNullOrEmpty(email))
                return Unauthorized();

            string? rawToken = Request.Headers["Authorization"]
                .ToString()
                .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            _logger.LogInformation("Revoke request for: {Email}", email);
            await _authService.RevokeAsync(email, rawToken);
            return NoContent();
        }

        /// <summary>Get current logged-in user info</summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var email = User.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.Email ||
                c.Type == "email")?.Value;

            _logger.LogInformation("Me request for: {Email}", email);

            return Ok(new
            {
                Id       = User.Claims.FirstOrDefault(c =>
                               c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Username = User.Identity?.Name,
                Email    = email,
                Role     = User.Claims.FirstOrDefault(c =>
                               c.Type == System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
