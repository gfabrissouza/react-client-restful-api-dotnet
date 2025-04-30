namespace RestApiDotNet.Configurations
{
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int Minutes { get; set; }
        public int DayToExpire { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string GoogleAuthorizationEndpoint { get; set; }
        public string GoogleValidateTokenEndpoint { get; set; }
    }
}
