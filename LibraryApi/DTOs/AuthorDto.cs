namespace LibraryApi.DTOs
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public List<BookDto> Books { get; set; }
    }

    public class AuthorCreateDto 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public List<BookDto> Books { get; set; }
    }

    public class AuthorUpdateDto 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public List<BookDto> Books { get; set; }
    }
}
