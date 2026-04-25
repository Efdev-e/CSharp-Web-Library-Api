
using LibraryApi.Models;
using LibraryApi.Services;
using Microsoft.AspNetCore.Http;
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


    }
}
