using System;

namespace BooksApp.Dto
{
    public class BookDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }

        public string[] Authors { get; set; }
    }
}
