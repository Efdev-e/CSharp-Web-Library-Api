using LibraryApi.Data;
using LibraryApi.DTOs; 
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly LibraryDBContext _context;

        public AuthorController(LibraryDBContext context)
        {
            _context = context;
        }



        [HttpGet(Name = "GetAuthors")]
        public ActionResult<IEnumerable<AuthorDto>> GetAll()
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

        [HttpGet("GetById")]
        public ActionResult<AuthorDto> GetById(int id)
        {
            if (id < 0)
                return BadRequest(new {error = "Invalid ID"});


            var Author = _context.Authors.Find(id);
            if (Author == null)
                return NotFound(new {error = "Unable to find request"});

            var authorDto = new AuthorDto
            {
                Id = Author.Id,
                FullName = Author.FullName,
                Country = Author.Country,
                Books = _context.Books
                    .Where(b => b.AuthorId == Author.Id)
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Category = b.Category,
                        Title = b.Title,
                        Year = b.Year,
                        AuthorName = Author.FullName
                    }).ToList()
            };

            return Ok(authorDto);
        }

        [HttpPost("CreateAuthor")]
        public ActionResult<AuthorDto> CreateAuthor([FromBody] AuthorCreateDto authorDto) 
        {
            var isValidAuthorId = authorDto.Id <= 0;
            var isValidAuthorName = string.IsNullOrEmpty(authorDto.FullName);
            var isValidAuthorCountry = string.IsNullOrEmpty(authorDto.Country);
            var isValidBook = authorDto.Books.All(b =>
                b.AuthorName == authorDto.FullName &&  
                b.Id > 0 &&                           
                b.Year > 1000 &&                     
                b.Year <= DateTime.Now.Year &&        
                !string.IsNullOrWhiteSpace(b.Category) &&
                !string.IsNullOrWhiteSpace(b.Title)
            );

            if (!isValidBook)
                return BadRequest(new { error = "Invalid Book" });
            if (isValidAuthorId)
                return BadRequest(new { error = "Invalid ID" });
            if (isValidAuthorName)
                return BadRequest(new { error = "Invalid Author Name" });
            if (isValidAuthorCountry)
                return BadRequest(new { error = "Invalid Author Country" });

            var newAuthor = new Author
            {
                Id = authorDto.Id,
                FullName = authorDto.FullName,
                Country = authorDto.Country,
                Books = authorDto.Books
                    .Select(b => new Book 
                    {
                        Title = b.Title,
                        Year = b.Year,
                        Category = b.Category,
                        AuthorId = authorDto.Id,                    
                    }).ToList()
            };

            _context.Authors.Add(newAuthor);
            _context.SaveChanges();

            var result = new AuthorDto
            {
                Id = newAuthor.Id,
                FullName = newAuthor.FullName,
                Country = newAuthor.Country,

                Books = newAuthor.Books.Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Year = b.Year,
                    Category = b.Category
                    
                }).ToList()
            };

            return Ok(result);
        }


        [HttpPut("Update/{id}")]
        public ActionResult<AuthorDto> Update(int id, [FromBody] AuthorCreateDto authorDto) 
        {
            var isValidID = id < 0;
            var isValidAuthorName = string.IsNullOrEmpty(authorDto.FullName);
            var isValidAuthorCountry = string.IsNullOrEmpty(authorDto.Country);


            if (id <= 0 || authorDto.Id != id)
                return BadRequest(new { error = "Invalid ID" });
            if (isValidAuthorName)
                return BadRequest(new { error = "Invalid Author Name" });
            if (isValidAuthorCountry)
                return BadRequest(new { error = "Invalid Author Country" });


            var updatingAuthor = _context.Authors.Find(id);

            if (updatingAuthor == null)
                return NotFound(new { error = $"Author with the given id {id} is Not Found" });

            updatingAuthor.FullName = authorDto.FullName;
            updatingAuthor.Country = authorDto.Country;
            updatingAuthor.Books = authorDto.Books
                .Select(b => new Book 
                {
                    Category = b.Category,
                    Title = b.Title,
                    Year = b.Year,
                    AuthorId = authorDto.Id
                }).ToList();

            _context.SaveChanges();
            // Showing Update

            return NoContent();
        }

        [HttpPut("Delete/{id}")]
        public ActionResult<AuthorDto> Delete(int id) 
        {
            var Author = _context.Authors.Find(id);

            if (Author == null)
                return NotFound(new { error = $"Author with the given id {id} is Not Found" });

            _context.Authors.Remove(Author);
            _context.SaveChanges();


            return NoContent();
        }
    }
}