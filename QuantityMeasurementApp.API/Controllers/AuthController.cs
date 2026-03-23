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
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>Register a new user</summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        /// <summary>Login and receive JWT tokens</summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        /// <summary>Get new access token using refresh token</summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDTO request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }

        /// <summary>Revoke refresh token (logout)</summary>
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

            await _authService.RevokeAsync(email);
            return NoContent();
        }

        /// <summary>Get current logged-in user info</summary>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            return Ok(new
            {
                Id       = User.Claims.FirstOrDefault(c =>
                               c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                Username = User.Identity?.Name,
                Email    = User.Claims.FirstOrDefault(c =>
                               c.Type == System.Security.Claims.ClaimTypes.Email ||
                               c.Type == "email")?.Value,
                Role     = User.Claims.FirstOrDefault(c =>
                               c.Type == System.Security.Claims.ClaimTypes.Role)?.Value
            });
        }
    }
}
