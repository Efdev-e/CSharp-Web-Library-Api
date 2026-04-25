

using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService) 
        {
            _tokenService = tokenService;
        }

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
            return Ok(new LoginResponse
            {
                Token = token,
                ExpiryDate = expiresAt,
                Username = request.Username,
                Role = user.Role
            });
        }
    }
}
