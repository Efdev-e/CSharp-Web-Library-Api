using LibraryApi.Data;
using LibraryApi.Models;
using LibraryApi.DTOs; // Make sure to include this!
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public AuthorController(LibraryDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetAuthors")]
        
        public ActionResult<IEnumerable<AuthorDto>> Get()
        {

            
            var allAuthors = _context.Authors
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Country = a.Country,
                    Books = _context.Books
                        .Where(b => b.AuthorId == a.Id)
                        .Select(b => new BookDto 
                        {
                            Title = b.Title,
                            Id = b.Id,
                            Year = b.Year,
                            AuthorName = b.Author.FullName,
                            Category = b.Category
                        }).ToList()
                    
                })
                .ToList();

            return Ok(allAuthors);
        }

        [HttpGet("debug-raw")]
        public IActionResult GetRaw()
        {
            var rawAuthors = _context.Authors.ToList();
            var count = _context.Authors.Count();
            return Ok(new { TotalInDb = count, Data = rawAuthors });
        }
    }
}