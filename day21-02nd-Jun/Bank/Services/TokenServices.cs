using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Services
{
    public class TokenService : ITokenServices
    {
        private readonly SymmetricSecurityKey _securityKey;
        public TokenService(IConfiguration configuration)
        {
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        }
        public async  Task<string> GenerateToken(Master master)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,master.UserName),
                new Claim(ClaimTypes.Role,master.Role)
            };
            var creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}