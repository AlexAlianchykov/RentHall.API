using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentHall.Application.AbstractionsServices;
using RentHall.Core.Models.AggregateUser;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentHall.Application.Services.Jwt
{
    public class JwtProvider: IJwtProvider
    {
        private JwtOptions _jwtOptions;
        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }
        public string GenerateToken(User user)
        {
            Claim[] clames = [
                new ("UserId", user.Id.ToString()),
                new ("UserRole", user.Role)];

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: clames,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours)
                );

            var ValueToken = new JwtSecurityTokenHandler().WriteToken(token);

            return ValueToken;
        }
    }
}
