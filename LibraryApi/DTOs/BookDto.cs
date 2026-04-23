namespace LibraryApi.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
        public string AuthorName { get; set; }
   
    }

    public class BookCreateDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
        public int AuthorId { get; set; }
    }
     public class BookUpdateDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Category { get; set; }
        public int AuthorId { get; set; }
    }
}
