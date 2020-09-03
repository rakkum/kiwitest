using System;
using System.Collections.Generic;

namespace BooksApp.Models
{
    public class Book
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        
        public virtual ICollection<BookAuthor> BookAuthors { get; set; }
        public virtual ICollection<BookHistory> BookHistories { get; set; }
    }
}
