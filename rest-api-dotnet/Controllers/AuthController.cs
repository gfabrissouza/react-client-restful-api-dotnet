using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiDotNet.Business;
using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public IActionResult SingIn([FromBody] UserVO user)
        {
            if (user == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidateCredentials(user);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh([FromBody] TokenVO tokenVO)
        {
            if (tokenVO == null) return BadRequest("Invalid client request");
            var token = _loginBusiness.ValidadeCredentials(tokenVO);
            if (token == null) return Unauthorized();
            return Ok(token);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var userName = User.Identity.Name;
            var result = _loginBusiness.RevokeToken(userName);
            if (!result) return BadRequest("Invalid client request");

            return NoContent();
        }

        [HttpGet]
        [Route("start")]
        public IActionResult StartLogin()
        {
            var state = Guid.NewGuid().ToString("N");

            Response.Cookies.Append("oauth_state", state, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                MaxAge = TimeSpan.FromMinutes(10)
            });

            return Redirect(_loginBusiness.GetGoogleLoginUrl(state));
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> CallbackAsync([FromQuery] string code, string state)
        {
            var expectedState = Request.Cookies["oauth_state"];
            if (state != expectedState)
            {
                return Unauthorized("Estado de autenticação inválido.");
            }

            var auth = await _loginBusiness.ProcessGoogleCallbackAsync(code, state);

            if (string.IsNullOrEmpty(auth.IdToken))
            {
                return Unauthorized("ID Token não retornado.");
            }

            // Validar id_token no Google
            var googlePayload = await _loginBusiness.ValidateIdTokenWithGoogle(auth.IdToken);
            if (googlePayload == null)
            {
                return Unauthorized("ID Token inválido.");
            }

            var email = googlePayload.Email;
            var name = googlePayload.Name;
            var picture = googlePayload.Picture;

            var token = _loginBusiness.ValidateUserByEmail(email);
            if (token == null)
            {
                return Unauthorized("Usuário inválido.");
            }

            // Store data on cookies to show in callback page
            Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.Parse(token.Expiration)
            });

            Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            Response.Cookies.Append("id_token", auth.IdToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            Response.Cookies.Append("user_name", name, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("user_email", email, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            Response.Cookies.Append("user_picture", picture, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Redirect($"{Request.Scheme}://{Request.Host}/callback.html");
        }
    }
}
