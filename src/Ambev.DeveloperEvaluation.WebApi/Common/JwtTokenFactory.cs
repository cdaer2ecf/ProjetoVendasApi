using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    public static class JwtTokenFactory
    {
        public static string CreateToken(
            IConfiguration config,
            string userId,          // EX: GUID do usuário
            string userName,        // EX: login/username
            IEnumerable<Claim>? extraClaims = null,
            TimeSpan? lifetime = null)
        {
            var issuer = config["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer ausente");
            var audience = config["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience ausente");
            var keyValue = config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key ausente");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var exp = now.Add(lifetime ?? TimeSpan.FromHours(2));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId),
                new(JwtRegisteredClaimNames.UniqueName, userName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (extraClaims != null) claims.AddRange(extraClaims);

            var jwt = new JwtSecurityToken(
                issuer: issuer,                 
                audience: audience,            
                claims: claims,
                notBefore: now,
                expires: exp,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
