using LearnApi.Config;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearnApi.Helper
{
    public class JwtHelper
    {
        private readonly JwtConfig config;
        public JwtHelper(IOptionsMonitor<JwtConfig> config)
        {
            this.config = config.CurrentValue;
        }
        public JwtSecurityToken GetToken(IEnumerable<Claim> claim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));
            return new JwtSecurityToken(
                issuer: config.ValidIssuer,
                audience: config.ValidAudience,
                expires: DateTime.Now.AddDays(1),
                claims: claim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha512)
                );
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidAudience = config.ValidAudience,
                ValidIssuer = config.ValidAudience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.Secret))
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var principal = jwtTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        public string GetRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
