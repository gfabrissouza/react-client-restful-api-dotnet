using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RestApiDotNet.Business;
using RestApiDotNet.Configurations;

namespace RestApiDotNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class ExternalAuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;
        private readonly ExternalAuthConfiguration _configuration;

        public ExternalAuthController(ExternalAuthConfiguration configuration, ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult SignIn()
        {
            var state = Guid.NewGuid().ToString("N");

            Response.Cookies.Append("oauth_state", state, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                MaxAge = TimeSpan.FromMinutes(10)
            });

            var url = _loginBusiness.GetGoogleLoginUrl(state);

            return Redirect(url);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CallbackAsync([FromQuery] string code, string state)
        {
            var expectedState = Request.Cookies["oauth_state"];
            if (state != expectedState)
            {
                return Unauthorized("Estado de autenticação inválido.");
            }

            var auth = await _loginBusiness.ProcessGoogleCallbackAsync(code);
            if (string.IsNullOrEmpty(auth.IdToken)) return Unauthorized("Invalid token");

            var googlePayload = await _loginBusiness.ValidateIdTokenWithGoogle(auth.IdToken);
            if (googlePayload == null) return Unauthorized("The token couldn't be validated");

            var token = _loginBusiness.ValidateUserByEmail(googlePayload.Email);
            if (token == null) return Unauthorized("Usuário inválido.");

            Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return Redirect(_configuration.FrontendRedirectUrl);
        }
    }
}
