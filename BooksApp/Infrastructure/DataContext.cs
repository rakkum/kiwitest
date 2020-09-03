using BooksApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Infrastructure
{
    public class DataContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookHistory> BookHistories { get; set; }

        public DataContext()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>()
                .HasOne(p => p.Book)
                .WithMany(p => p.BookAuthors)
                .HasForeignKey(p => p.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(p => p.Author)
                .WithMany(p => p.BooksAuthors)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<Book>()
                .HasMany(p => p.BookHistories)
                .WithOne(p => p.Book)
                .HasForeignKey(p => p.BookId);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(x => new {x.BookId, x.AuthorId});
            
            modelBuilder.Entity<Book>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Author>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<BookHistory>()
                .HasKey(p => p.Id);

            Seeder.SeedData(modelBuilder, @"books.json");
        }
    }
}
