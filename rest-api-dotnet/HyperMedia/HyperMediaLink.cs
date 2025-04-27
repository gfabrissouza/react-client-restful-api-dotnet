using System.Text;

namespace RestApiDotNet.HyperMedia
{
    public class HyperMediaLink
    {
        public string Rel { get; set; }
        private string href;
        public string Href
        {
            get 
            {
                object _lock = new object();
                lock (_lock)
                {
                    StringBuilder sb = new StringBuilder(href);
                    return sb.Replace("%2F", "/").ToString();
                }
            }
            set
            {
                href = value;
            }
        } 
        public string Action { get; set; }
        public string Type { get; set; }
    }
}
