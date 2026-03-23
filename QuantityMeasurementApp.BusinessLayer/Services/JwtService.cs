using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.BusinessLayer.Services
{
    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int    _accessTokenExpiryMinutes;
        private readonly int    _refreshTokenExpiryDays;

        public JwtService(
            string secretKey,
            string issuer,
            string audience,
            int    accessTokenExpiryMinutes = 60,
            int    refreshTokenExpiryDays   = 7)
        {
            _secretKey                = secretKey;
            _issuer                   = issuer;
            _audience                 = audience;
            _accessTokenExpiryMinutes = accessTokenExpiryMinutes;
            _refreshTokenExpiryDays   = refreshTokenExpiryDays;
        }

        public string GenerateAccessToken(UserEntity user)
        {
            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,               user.Username),
                new Claim(ClaimTypes.Role,               user.Role)
            };

            var token = new JwtSecurityToken(
                issuer:             _issuer,
                audience:           _audience,
                claims:             claims,
                expires:            DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer           = true,
                ValidateAudience         = true,
                ValidateLifetime         = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer              = _issuer,
                ValidAudience            = _audience,
                IssuerSigningKey         = new SymmetricSecurityKey(
                                               Encoding.UTF8.GetBytes(_secretKey))
            };

            var handler   = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(
                                token, parameters, out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwt ||
                !jwt.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
                return null;

            return principal;
        }
    }
}