using RestApiDotNet.HyperMedia;

namespace RestApiDotNet.Hypermedia.Abstract
{
    public interface ISupportsHyperMedia
    {
        List<HyperMediaLink> Links { get; set; }
    }
}
