using BooksApp.Dto;

namespace BooksApp.Services
{
    public interface IBookService
    {
        BookDto Update(long id, BookUpdateDto update);
    }
}