using RestApiDotNet.Hypermedia.Abstract;

namespace RestApiDotNet.HyperMedia.Utils
{
    public class PagedSearchVO<T> where T : ISupportsHyperMedia
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalResults { get; set; }
        public string SortFields { get; set; }
        public string SortDirections { get; set; }
        public Dictionary<string, object> Filters { get; set; } = new Dictionary<string, object>();
        public List<T> List { get; set; } = new List<T>();
        public PagedSearchVO()
        {
        }
        public PagedSearchVO(int currentPage, int pageSize, string sortFields, string sortDirections)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            SortFields = sortFields;
            SortDirections = sortDirections;
        }
        public PagedSearchVO(int currentPage, int pageSize, int totalResults, string sortFields, string sortDirections)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalResults = totalResults;
            SortFields = sortFields;
            SortDirections = sortDirections;
        }
        public PagedSearchVO(int currentPage, int pageSize, int totalResults, string sortFields, string sortDirections, Dictionary<string, object> filters)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalResults = totalResults;
            SortFields = sortFields;
            SortDirections = sortDirections;
            Filters = filters;
        }
        public PagedSearchVO(int currentPage, string sortFields, string sortDirections) 
            : this(currentPage, 10, sortFields, sortDirections)
        {
        }
    }
}
