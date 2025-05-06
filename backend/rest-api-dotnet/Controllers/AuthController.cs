using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiDotNet.Business;
using RestApiDotNet.Data.VO;
using System.Security.Claims;

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

            Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            Response.Cookies.Append("refresh_token", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return NoContent();
        }

        [HttpPost]
        [Route("refresh")]
        public IActionResult Refresh()
        {
            var accessToken = Request.Cookies["access_token"];
            if (string.IsNullOrEmpty(accessToken)) return Unauthorized();

            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken)) return Unauthorized();

            var tokenVO = new TokenVO(true, string.Empty, string.Empty, accessToken, refreshToken);
            var token = _loginBusiness.ValidadeCredentials(tokenVO);
            if (token == null) return Unauthorized();

            Response.Cookies.Append("access_token", token.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });

            return NoContent();
        }

        [HttpGet("me")]
        [Authorize("Bearer")]
        public IActionResult Me()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userClaims = identity.Claims;

            var userInfo = new
            {
                Name = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                Email = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value
            };

            return Ok(userInfo);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public IActionResult Revoke()
        {
            var userName = User.Identity?.Name;

            if (string.IsNullOrEmpty(userName) || !_loginBusiness.RevokeToken(userName))
            {
                return BadRequest("Invalid client request");
            }

            var result = _loginBusiness.RevokeToken(userName);
            if (!result) return BadRequest("Request failed");

            Response.Cookies.Delete("access_token", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            Response.Cookies.Delete("refresh_token", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            Response.Cookies.Delete("oauth_state", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });
            
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
                SameSite = SameSiteMode.None,
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

            return Redirect("http://localhost:5173/callback");
        }
    }
}
