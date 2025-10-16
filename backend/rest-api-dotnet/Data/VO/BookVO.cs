using RestApiDotNet.Hypermedia.Abstract;
using RestApiDotNet.HyperMedia;

namespace RestApiDotNet.Model
{
    public class BookVO : ISupportsHyperMedia
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public DateTime LaunchDate { get; set; }
        public decimal Price { get; set; }
        public string Title { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
