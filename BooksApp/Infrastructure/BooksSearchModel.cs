using System;

namespace BooksApp.Infrastructure
{
    public class BooksSearchModel : ISearchModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public string SortColumn { get; set; }
        public string SortDir { get; set; }
    }
}
