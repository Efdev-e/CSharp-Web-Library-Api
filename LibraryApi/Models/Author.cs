namespace LibraryApi.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        //Navigation property for related books
        public List<Book> Books { get; set; } = new List<Book>();
    }
}
