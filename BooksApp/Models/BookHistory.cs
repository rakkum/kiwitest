using System;

namespace BooksApp.Models
{
    public class BookHistory
    {
        public long Id { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        public DateTime TimestampUtc { get; set; }
        
        public long BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
