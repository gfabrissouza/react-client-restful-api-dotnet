using RestApiDotNet.Hypermedia.Abstract;
using RestApiDotNet.HyperMedia;

namespace RestApiDotNet.Data.VO
{
    public class PersonVO : ISupportsHyperMedia
    {
        public long Id { get; set; }
        //[JsonPropertyName("name")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        //[JsonIgnore]
        public string Address { get; set; }
        //[JsonPropertyName("sex")]
        public string Gender { get; set; }
        public bool Enabled { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
