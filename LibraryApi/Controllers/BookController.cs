using LibraryApi.Data;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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

        [HttpPost("CreateBook")]
        public ActionResult<BookCreateDto> Create([FromBody] BookCreateDto bookCreateDto) 
        {
            if (string.IsNullOrEmpty(bookCreateDto.Title))
                return BadRequest(new { error = "Title is required" });
            if (bookCreateDto.Year < 1000 || bookCreateDto.Year > DateTime.Now.Year || bookCreateDto.Year == null)
                return BadRequest(new { error = "Year is not valid" });

            var authorExists = _context.Authors.Any(a => a.Id == bookCreateDto.AuthorId);
            if (!authorExists)
                return BadRequest(new { error = $"Author with id {bookCreateDto.AuthorId} not found" });

            var book = new Book
            {
                Title = bookCreateDto.Title,
                AuthorId = bookCreateDto.AuthorId,
                Year = bookCreateDto.Year,
                Category = bookCreateDto.Category
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            _context.Entry(book).Reference(b => b.Author).Load();

            var result = new BookDto
            {
                Title = book.Title,
                Year = book.Year,
                AuthorName = book.Author?.FullName,
                Category = book.Category
            };
            
            return CreatedAtAction(nameof(GetById), new {id = book.Id},result); // 201 created
        }
        [HttpPut("Update/{id}")]
        public ActionResult<BookUpdateDto> Update(int id, [FromBody] BookUpdateDto updateDto) 
        {
            var book = _context.Books.Find(id);
            if (book == null)
                return NotFound();
            if (string.IsNullOrWhiteSpace(updateDto.Title))
                return BadRequest(new { error = "Title is required" });

            book.Title = updateDto.Title;
            book.Year = updateDto.Year;
            book.Category = updateDto.Category;
            book.AuthorId = updateDto.AuthorId;
            _context.SaveChanges();
            return NoContent(); // 204
        }

        [HttpPut("Delete/{id}")]
        public ActionResult<BookUpdateDto> Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
                return NotFound();


            _context.Books.Remove(book);
            _context.SaveChanges();
            return NoContent(); // 204
        }
    }
}
