using System.Collections.Generic;

namespace BooksApp.Models
{
    public class Author
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        
        public virtual ICollection<BookAuthor> BooksAuthors { get; set; }
    }
}
