namespace BooksApp.Models
{
    public class BookAuthor
    {
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
        public long AuthorId { get; set; }
        public virtual Author Author { get; set; }
    }
}