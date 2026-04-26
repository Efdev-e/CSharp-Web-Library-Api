using LibraryApi.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace LibraryApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        public TokenService(JwtSettings jwtSettings) 
        {
            _jwtSettings = jwtSettings;
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return Convert.ToBase64String(randomBytes);
            // Bu rastgele 64 byte üretip Base64 String'e çeviriyor
        }

        public (string Token, DateTime ExpiresAt) GenerateToken(User user)
        {
            // Token içerinsdeki data
            var claims = new List<Claim> 
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,user.Role)
            };

            // Key

            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var key = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // Token Bitiş Zamanı

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes);

            // Token Oluştur

            var token = new JwtSecurityToken(
                    issuer: _jwtSettings.Issuer,
                    audience: _jwtSettings.Audience,
                    claims: claims,
                    expires: expiresAt,
                    signingCredentials: creds
                    );

            //string çevir

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expiresAt);
        }
    }
}
