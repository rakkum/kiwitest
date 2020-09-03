using System;
using System.Collections.Generic;
using System.Linq;
using BooksApp.Dto;
using BooksApp.Infrastructure;
using BooksApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApp.Queries
{
    public class BookQueries : IBookQueries
    {
        private readonly DataContext _dataContext;

        public BookQueries(DataContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public PagedResult<BookDto> GetAll(BooksSearchModel searchModel)
        {
            var query = _dataContext.Books
                .Include(p => p.BookAuthors)
                .ThenInclude(p => p.Author)
                .AsQueryable();

            var binder = new SearchBinder<BooksSearchModel, Book>()
                .AddSearchMapping(p => p.Description, (book, sm) => book.Description.Contains(sm.Description, StringComparison.InvariantCultureIgnoreCase))
                .AddSearchMapping(p => p.Title, (book, sm) => book.Title.Contains(sm.Title, StringComparison.InvariantCultureIgnoreCase));

            query = binder
                .ApplySearch(query, searchModel)
                .OrderBy(searchModel);

            var totalItems = query.Count();

            query = query.SkipAndTake(searchModel);
            
            var result = new PagedResult<BookDto>
            {
                ItemCount = totalItems,
                Items =query.Select(p => new BookDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    PublishDate = p.PublishDate,
                    Authors = p.BookAuthors.Select(o => o.Author.Name).ToArray()
                }).ToArray() 
            };

            return result;

        }
        
        public IEnumerable<BookHistory> GetHistory(long id)
        {
            return _dataContext.BookHistories.Where(p => p.BookId == id).ToArray();
        }
    }
}
