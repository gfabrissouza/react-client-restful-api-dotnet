namespace RestApiDotNet.Configurations
{
    public class AuthConfiguration
    {
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        public string RedirectUri { get; set; }
        public string GoogleAuthorizationEndpoint { get; set; }
        public string GoogleValidateTokenEndpoint { get; set; }
        public string FrontendRedirectUrl { get; set; }
    }
}
