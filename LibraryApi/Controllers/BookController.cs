using LibraryApi.Data;
using LibraryApi.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public BookController(LibraryDBContext context) 
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<BookDto>> GetAll([FromQuery] string? category, [FromQuery] int? year) 
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(b => b.Category == category);

            if (year.HasValue)
                query = query.Where(b => b.Year == year);

            var books = query.Select(b => new BookDto 
            { 
                Id = b.Id,
                Title = b.Title,
                Category = b.Category,
                AuthorName = b.Author!.FullName,
                Year = b.Year,
            }).ToList();

            return Ok(books);
        }

        [HttpGet("GetById")]
        public ActionResult<BookDto> GetById([FromQuery] int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .FirstOrDefault(x => x.Id == id);

            if (book == null)
                return NotFound(new { error = $"Book with id {id} not found" });

            var dto = new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Category = book.Category,
                AuthorName = book.Author?.FullName,
                Year = book.Year,
            };

            return Ok(dto);
        }
    }
}
