using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestApiDotNet.Configurations;
using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Services.Implementations
{
    public class GoogleOAuthService : IAuthService
    {
        private TokenConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleOAuthService(TokenConfiguration configuration, IHttpClientFactory factory)
        {
            _httpClientFactory = factory;
            _configuration = configuration;
        }

        public string GetGoogleLoginUrl(string state)
        {
            var scope = "openid email profile";

            return $"{_configuration.GoogleAuthorizationEndpoint}?client_id={_configuration.GoogleClientId}" +
                   $"&redirect_uri={Uri.EscapeDataString(_configuration.RedirectUri)}" +
                   $"&response_type=code" +
                   $"&scope={Uri.EscapeDataString(scope)}" +
                   $"&state={state}";
        }

        public async Task<AuthResponseVO> ProcessGoogleCallbackAsync(string code)
        {
            var http = _httpClientFactory.CreateClient();
            var tokenRequest = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _configuration.GoogleClientId },
                { "client_secret", _configuration.GoogleClientSecret },
                { "redirect_uri", _configuration.RedirectUri },
                { "grant_type", "authorization_code" }
            };

            var response = await http.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequest));
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var idToken = json["id_token"]?.ToString();

            return new AuthResponseVO { IdToken = idToken };
        }

        public async Task<GoogleIdTokenPayloadVO> ValidateIdTokenWithGoogle(string idToken)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_configuration.GoogleValidateTokenEndpoint}{idToken}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<GoogleIdTokenPayloadVO>(content);

            if (payload == null)
                return null;

            // Validação do "aud"
            if (payload.Aud != _configuration.GoogleClientId)
                return null;

            // Validação do "iss"
            if (payload.Iss != "accounts.google.com" && payload.Iss != "https://accounts.google.com")
                return null;

            // Validação de expiração
            var expDate = DateTimeOffset.FromUnixTimeSeconds(payload.Exp).UtcDateTime;
            if (expDate < DateTime.UtcNow)
                return null;

            return payload;
        }
    }
}

