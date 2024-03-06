using AlpataEntities.Entities.Concretes;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AlpataBLL.Utilities.Security.Jwt
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration Configuration;

        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Token CreateToken(AppUser user)
        {
            Token token = new();

            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"]));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

            token.Expiration = DateTime.Now.AddYears(1);
            JwtSecurityToken securityToken = new(
                issuer: Configuration["Token:Issuer"],
                audience: Configuration["Token:Audience"],
                expires: token.Expiration,
                claims: SetClaims(user),
                notBefore: DateTime.Now,
                signingCredentials: signingCredentials
                );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

            token.AccessToken = jwtSecurityTokenHandler.WriteToken(securityToken);

            token.RefreshToken = CreateRefreshToken();
            return token;
        }

        public static string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }

        private static IEnumerable<Claim> SetClaims(AppUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, string.Join(' ', user.Name, user.Surname)),
                new("Id", user.Id.ToString()),
                new(ClaimTypes.Email, user?.Email ?? string.Empty)
            };

            return claims;
        }

        public static bool ValidateJwtToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true
            };

            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
