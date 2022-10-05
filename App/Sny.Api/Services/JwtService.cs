using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sny.Api.Options;
using Sny.Core.AccountsAggregate;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sny.Api.Services
{
    public interface IJwtService
    {
        (string Jwt, DateTime expiryAt) CreateJWT(Account acc);
        (string Jwt, string RefreshTokenId, DateTime expiryAt) CreateRefreshJwt();
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal? ValidateJwtAndGetPrincipal(string token);
    }
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _jwtOptions;

        public JwtService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Generate short-live JWT token for accessing to API.
        /// JWT token contains User email and ID.
        /// Expiration is loaded from configuration key Jwt:TokenValidityInMinutes, default 10 minutes.
        /// </summary>
        /// <param name="acc"></param>
        /// <returns></returns>
        public (string Jwt, DateTime expiryAt) CreateJWT(Account acc)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            var expiryAt = DateTime.UtcNow.AddMinutes(_jwtOptions.TokenValidityInMinutes);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, acc.Email),
                new Claim(JwtRegisteredClaimNames.Sub, acc.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, acc.Email),
                new Claim(JwtRegisteredClaimNames.Jti, acc.Id.ToString())
            };

            var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer, 
                audience: _jwtOptions.Audience, 
                claims: claims, 
                expires: expiryAt, 
                signingCredentials: credentials);
            return (new JwtSecurityTokenHandler().WriteToken(token), expiryAt);
        }

        /// <summary>
        /// Generates long-life refresh token usable for generate JWT (after its expiration).
        /// Refresh token is JWT; and expiry date is validated as usual from content of JWT (it is not stored in databse).
        /// Expiration is loaded from configuration key Jwt:RefreshTokenValidityInDays, default 7 days.
        /// 
        /// Jwt token contains generated unique ID, which is also return from this method and it can be used for check token validity 
        /// (this ID can be persisted as 'valid refresh token' at user entity)
        /// </summary>
        /// <returns></returns>
        public (string Jwt, string RefreshTokenId, DateTime expiryAt) CreateRefreshJwt()
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var refreshTokenId = Guid.NewGuid().ToString();
            var expiryAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenValidityInDays);
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, refreshTokenId),
            };

            var token = new JwtSecurityToken(issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiryAt,
                signingCredentials: credentials);
            return (new JwtSecurityTokenHandler().WriteToken(token), refreshTokenId, expiryAt);
        }

        /// <summary>
        /// returns null if not validated
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal? ValidateJwtAndGetPrincipal(string token)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var settings = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretkey,
                ValidAudience = _jwtOptions.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(10)
            };

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, settings, out var securityToken);

            if (securityToken is not JwtSecurityToken) return null;
            return principal;
        }

        /// <summary>
        /// returns null if not validated
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtOptions.Secret));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretkey,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken) return null;

            return principal;
        }
    }
}
