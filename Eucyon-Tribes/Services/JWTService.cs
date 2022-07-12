﻿using Eucyon_Tribes.Models;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

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
            var myKey = _config["TokenGenerationKey"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(myKey);
            var tokenDescriptor = new SecurityTokenDescriptor();
            if (purpose.Equals("verify"))
            {
                tokenDescriptor.Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) });
                tokenDescriptor.Expires = user.VerificationTokenExpiresAt;
                tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            }
            else if (purpose.Equals("forgotten password"))
            {
                tokenDescriptor.Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) });
                tokenDescriptor.Expires = user.ForgottenPasswordTokenExpiresAt;
                tokenDescriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int ValidateToken(string token)
        {
            if (token == null)
            {
                return -1;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var myKey = _config["TokenGenerationKey"];
            var key = Encoding.ASCII.GetBytes("THE MOST SECRET SECRET USED TO SIGN AND VERIFY JWT TOKENS, THAT IS DEFINITELY VERY SECURE");
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
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                return userId;
            }
            catch
            {
                return -1;
            }
        }
    }
}