

using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService) 
        {
            _tokenService = tokenService;
        }

        private static readonly List<RefreshToken> _refreshTokens = new();

        private static readonly List<User> _users = new()
        {
            new User {Id=1,Username="efe",Password="1234",Role="admin" },
            new User { Id = 2, Username = "ayse", Password = "pass123", Role = "user" },
            new User { Id = 3, Username = "mehmet", Password = "qwerty", Role = "user" },
            new User { Id = 4, Username = "zeynep", Password = "abc123", Role = "moderator" },
            new User { Id = 5, Username = "can", Password = "9876", Role = "admin" }
        };

        [HttpPost("login")]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new {error= "Username and Password required"});

            var user = _users.FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);

            if (user == null)
                return Unauthorized(new { error = "Username or Password is Invalid" });

            var(token,expiresAt) = _tokenService.GenerateToken(user);

            var refreshToken = _tokenService.GenerateRefreshToken();
            _refreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked=false,
                CreatedAts= DateTime.UtcNow
            });

            return Ok(new LoginResponse
            {
                Token = token,
                ExpiryDate = expiresAt,
                RefreshToken = refreshToken,
                Username = request.Username,
                Role = user.Role
            });
        }

        [HttpPost("refresh")]
        public ActionResult<LoginResponse> Refresh([FromBody] RefreshRequest request) 
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest(new { error = "Refresh Token is required" });

            var stored = _refreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);

            if (stored == null)
                return Unauthorized(new { error = "Invalid refresh token" });

            if (stored.IsRevoked)
                    return Unauthorized(new { error = "Refresh Token has been revoked" });

            if (stored.ExpiresAt < DateTime.UtcNow)
                return Unauthorized(new { error = "Refresh Token has expired" });

            var user = _users.FirstOrDefault(x => x.Id == stored.UserId);
            if (user == null)
                return Unauthorized(new {error = "User not found"});

            stored.IsRevoked = true;

            var (token, expiresAt) = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            _refreshTokens.Add(new RefreshToken 
            {
                Token = newRefreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                CreatedAts = DateTime.UtcNow
            });

            return Ok(new LoginResponse 
            {
                Token = token,
                ExpiryDate = expiresAt,
                RefreshToken = newRefreshToken,
                Username = user.Username,
                Role = user.Role
            });
        }


    }
}
