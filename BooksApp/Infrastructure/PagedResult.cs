using System.Collections.Generic;

namespace BooksApp.Infrastructure
{
    public class PagedResult<T>
    {
        public int ItemCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
