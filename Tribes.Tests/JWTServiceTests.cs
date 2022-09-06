using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class JWTServiceTests
    {
        public IAuthService AuthService { get; set; }
        public IConfiguration Config { get; set; }

        public JWTServiceTests()
        {
            var Config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            AuthService = new JWTService(this.Config);
        }

        [Theory]
        [InlineData(null, -1)]
        [InlineData("ab", -1)]
        public void ValidateTokenWithWrongInputs(string input, int expected)
        {
            int result = AuthService.ValidateToken(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateTokenTest()
        {
            var expiresAt = DateTime.UtcNow.AddHours(1);
            var user = new User() { Id = 1, VerificationTokenExpiresAt = expiresAt };
            user.Role = "Player";
            user.Name = "Player";
            user.Email = "Player";
            var usersToken = AuthService.GenerateToken(user, "verify");

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TokenGenerationKey")!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, "1") ,
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.Name)
                };

            var token = new JwtSecurityToken(null, null, claims, null, expires: user.VerificationTokenExpiresAt,
                signingCredentials: credentials);
            var result = tokenHandler.WriteToken(token);

            Assert.Equal(result, usersToken);
        }
    }
}
