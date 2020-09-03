namespace BooksApp.Infrastructure
{
    public interface ISearchModel
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        string SortColumn { get; set; }
        string SortDir { get; set; }
    }
}