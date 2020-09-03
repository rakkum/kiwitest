using System.Collections.Generic;
using BooksApp.Dto;
using BooksApp.Infrastructure;
using BooksApp.Models;

namespace BooksApp.Queries
{
    public interface IBookQueries
    {
        PagedResult<BookDto> GetAll(BooksSearchModel searchModel);
        IEnumerable<BookHistory> GetHistory(long id);
    }
}
