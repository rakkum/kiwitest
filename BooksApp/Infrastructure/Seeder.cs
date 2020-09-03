using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BooksApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BooksApp.Infrastructure
{
    public static class Seeder
    {
        public static void SeedData(ModelBuilder modelBuilder, string jsonDataSource)
        {
            var fileContents = File.ReadAllText(jsonDataSource);
            var sourceBooks = JsonConvert.DeserializeObject<BookModel[]>(fileContents,
                new IsoDateTimeConverter
                {
                    Culture = CultureInfo.InvariantCulture,
                    DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffzzz"
                })!.Where(p => p.PublishedDate != null).ToArray();

            var authors = sourceBooks.SelectMany(p => p.Authors)
                .Distinct()
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select((author, index) => new Author
                {
                    Id = index + 1,
                    Name = author
                }).ToArray();

            var books = sourceBooks.Select((book, index) => new Book
            {
                Id = index + 1,
                Title = book.Title,
                Description = book.ShortDescription,
                PublishDate = book.PublishedDate.Date
            }).ToArray();

            var relations = new List<BookAuthor>();
            foreach (var author in authors)
            {
                var bookAuthors = sourceBooks.Where(p => p.Authors.Contains(author.Name))
                    .Select(p => books.First(o => o.Title == p.Title))
                    .Select(p => new BookAuthor
                    {
                        AuthorId = author.Id,
                        BookId = p.Id
                    });

                relations.AddRange(bookAuthors);
            }
            modelBuilder.Entity<Book>().HasData(books);
            modelBuilder.Entity<Author>().HasData(authors);
            modelBuilder.Entity<BookAuthor>().HasData(relations);
        }


        private class BookModel
        {
            public string Title { get; set; }
            public string ShortDescription { get; set; }
            public DateModel PublishedDate { get; set; }
            public string[] Authors { get; set; }
        }

        private class DateModel
        {
            [JsonProperty("$date")] 
            public DateTime Date { get; set; }
        }
    }
}
