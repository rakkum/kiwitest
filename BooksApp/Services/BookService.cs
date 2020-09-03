using System;
using System.Collections.Generic;
using System.Linq;
using BooksApp.Dto;
using BooksApp.Infrastructure;
using BooksApp.Models;

namespace BooksApp.Services
{
    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;

        public BookService(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public BookDto Update(long id, BookUpdateDto update)
        {
            var existing = _dataContext.Books
                .FirstOrDefault(p => p.Id == id);

            if (existing == null)
            {
                // TODO: don't throw that
                throw new Exception();
            }

            var history = UpdateAndGetHistory(existing, update);

            _dataContext.BookHistories.AddRange(history);

            _dataContext.SaveChanges();
            
            return new BookDto
            {
                Id = id,
                Title = existing.Title,
                Description = existing.Description,
                PublishDate = existing.PublishDate
            };
        }

        private static IEnumerable<BookHistory> UpdateAndGetHistory(Book source, BookUpdateDto update)
        {
            if (source.Title != update.Title)
            {
                source.Title = update.Title;
                yield return new BookHistory
                {
                    BookId = source.Id,
                    FieldName = nameof(source.Title),
                    OldValue = source.Title,
                    NewValue = update.Title,
                    TimestampUtc = DateTime.UtcNow
                };
            }
            
            if (source.Description != update.Description)
            {
                source.Description = update.Description;
                yield return new BookHistory
                {
                    BookId = source.Id,
                    FieldName = nameof(source.Description),
                    OldValue = source.Description,
                    NewValue = update.Description,
                    TimestampUtc = DateTime.UtcNow
                };
            }
        }
    }
}
