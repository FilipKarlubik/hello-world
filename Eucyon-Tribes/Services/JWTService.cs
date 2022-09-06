using Eucyon_Tribes.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eucyon_Tribes.Services
{
    public class JWTService : IAuthService
    {
        private readonly IConfiguration _config;
        public JWTService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user, string purpose)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
                var claims = new[] { 
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) ,
                    new Claim(ClaimTypes.Role, user.Role), 
                    new Claim(ClaimTypes.Email, user.Email), 
                    new Claim(ClaimTypes.Name, user.Name) 
                };

            var token = new JwtSecurityToken(null,null,claims,null, expires: purpose.Equals("verify") ? user.VerificationTokenExpiresAt : user.ForgottenPasswordTokenExpiresAt,
                signingCredentials: credentials);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateToken(string token)
        {
            if (token == null)
            {
                return -1;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey"));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;
                var userId = int.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value);
                return userId;
            }
            catch
            {
                return -1;
            }
        }
    }
}
