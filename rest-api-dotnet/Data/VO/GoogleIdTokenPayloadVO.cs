namespace RestApiDotNet.Data.VO
{
    public class GoogleIdTokenPayloadVO
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
        public long Exp { get; set; }
    }
}