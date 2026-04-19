namespace LibraryApi.Models
{
    public class Book 
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public string Category { get; set; } = null!;

        // Foreign Key
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;
    }
}
