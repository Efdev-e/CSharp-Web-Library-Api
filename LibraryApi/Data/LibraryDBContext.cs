using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;

namespace LibraryApi.Data
{
    public class LibraryDBContext : DbContext
    {
      
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();

        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options) 
        {
        
        }


      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Book>()
                .HasKey(e => e.Id);


            modelBuilder.Entity<Book>()
                .HasOne(e => e.Author)
                .WithMany(e => e.Books)
                .HasForeignKey(e => e.AuthorId);


            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "1984", Year = 1949, Category = "Dystopian", AuthorId = 1 },
                new Book { Id = 2, Title = "Pride and Prejudice", Year = 1813, Category = "Romance", AuthorId = 2 },
                new Book { Id = 3, Title = "Huckleberry Finn", Year = 1884, Category = "Adventure", AuthorId = 3 },
                new Book { Id = 4, Title = "Crime and Punishment", Year = 1866, Category = "Philosophical", AuthorId = 4 },
                new Book { Id = 5, Title = "Norwegian Wood", Year = 1987, Category = "Romance", AuthorId = 5 },
                new Book { Id = 6, Title = "The Old Man and the Sea", Year = 1952, Category = "Fiction", AuthorId = 6 },
                new Book { Id = 7, Title = "War and Peace", Year = 1869, Category = "Historical", AuthorId = 7 },
                new Book { Id = 8, Title = "Harry Potter 1", Year = 1997, Category = "Fantasy", AuthorId = 8 },
                new Book { Id = 9, Title = "Murder on the Orient Express", Year = 1934, Category = "Mystery", AuthorId = 9 },
                new Book { Id = 10, Title = "The Shining", Year = 1977, Category = "Horror", AuthorId = 10 },
                new Book { Id = 11, Title = "Oliver Twist", Year = 1839, Category = "Drama", AuthorId = 11 },
                new Book { Id = 12, Title = "Mrs Dalloway", Year = 1925, Category = "Modernist", AuthorId = 12 },
                new Book { Id = 13, Title = "The Trial", Year = 1925, Category = "Absurdist", AuthorId = 13 },
                new Book { Id = 14, Title = "The Time Machine", Year = 1895, Category = "Sci-Fi", AuthorId = 14 },
                new Book { Id = 15, Title = "Foundation", Year = 1951, Category = "Sci-Fi", AuthorId = 15 },
                new Book { Id = 16, Title = "Sherlock Holmes", Year = 1892, Category = "Detective", AuthorId = 16 },
                new Book { Id = 17, Title = "The Da Vinci Code", Year = 2003, Category = "Thriller", AuthorId = 17 },
                new Book { Id = 18, Title = "The Alchemist", Year = 1988, Category = "Adventure", AuthorId = 18 },
                new Book { Id = 19, Title = "The Kite Runner", Year = 2003, Category = "Drama", AuthorId = 19 },
                new Book { Id = 20, Title = "The Hunger Games", Year = 2008, Category = "Dystopian", AuthorId = 20 }
            );

            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "George Orwell", Country = "UK" },
                new Author { Id = 2, FullName = "Jane Austen", Country = "UK" },
                new Author { Id = 3, FullName = "Mark Twain", Country = "US" },
                new Author { Id = 4, FullName = "Fyodor Dostoevsky", Country = "Russia" },
                new Author { Id = 5, FullName = "Haruki Murakami", Country = "Japan" },
                new Author { Id = 6, FullName = "Ernest Hemingway", Country = "US" },
                new Author { Id = 7, FullName = "Leo Tolstoy", Country = "Russia" },
                new Author { Id = 8, FullName = "J.K. Rowling", Country = "UK" },
                new Author { Id = 9, FullName = "Agatha Christie", Country = "UK" },
                new Author { Id = 10, FullName = "Stephen King", Country = "US" },
                new Author { Id = 11, FullName = "Charles Dickens", Country = "UK" },
                new Author { Id = 12, FullName = "Virginia Woolf", Country = "UK" },
                new Author { Id = 13, FullName = "Franz Kafka", Country = "Czech Republic" },
                new Author { Id = 14, FullName = "H.G. Wells", Country = "UK" },
                new Author { Id = 15, FullName = "Isaac Asimov", Country = "US" },
                new Author { Id = 16, FullName = "Arthur Conan Doyle", Country = "UK" },
                new Author { Id = 17, FullName = "Dan Brown", Country = "US" },
                new Author { Id = 18, FullName = "Paulo Coelho", Country = "Brazil" },
                new Author { Id = 19, FullName = "Khaled Hosseini", Country = "Afghanistan" },
                new Author { Id = 20, FullName = "Suzanne Collins", Country = "US" }
            );

        }
    }
}
