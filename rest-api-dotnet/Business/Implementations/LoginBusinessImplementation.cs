using RestApiDotNet.Configurations;
using RestApiDotNet.Data.VO;
using RestApiDotNet.Model.Repository;
using RestApiDotNet.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestApiDotNet.Business.Implementations
{
    public class LoginBusinessImplementation : ILoginBusiness
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfiguration _configuration;

        private readonly IUserRepository _repository;
        private readonly ITokenService _tokenService;
        private readonly IAuthService _authService;

        public LoginBusinessImplementation(TokenConfiguration configuration, IUserRepository repository, ITokenService tokenService, IAuthService authService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
            _authService = authService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _repository.ValidateCredentials(userCredentials);
            if (user == null) return null;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.DayToExpire);

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.UtcNow;
            DateTime expirationDate = DateTime.UtcNow.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                authenticated: true,
                created: createdDate.ToString(DATE_FORMAT),
                expiration: expirationDate.ToString(DATE_FORMAT),
                accessToken: accessToken,
                refreshToken: refreshToken);

        }

        public TokenVO ValidadeCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userName = principal.Identity.Name;

            var user = _repository.ValidateCredentials(userName);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.UtcNow;
            DateTime expirationDate = DateTime.UtcNow.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                true,
                createdDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken);
        }

        public bool RevokeToken(string userName)
        {
            return _repository.RevokeToken(userName);
        }

        public string GetGoogleLoginUrl(string state)
        {
            return _authService.GetGoogleLoginUrl(state);
        }

        public Task<AuthResponseVO> ProcessGoogleCallbackAsync(string code, string state)
        {
            return _authService.ProcessGoogleCallbackAsync(code, state);
        }

        public Task<GoogleIdTokenPayloadVO> ValidateIdTokenWithGoogle(string idToken)
        {
            return _authService.ValidateIdTokenWithGoogle(idToken);
        }

        public TokenVO ValidateUserByEmail(string email)
        {
            var user = _repository.GetUserByEmail(email);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.DayToExpire);

            _repository.RefreshUserInfo(user);

            DateTime createdDate = DateTime.UtcNow;
            DateTime expirationDate = DateTime.UtcNow.AddMinutes(_configuration.Minutes);

            return new TokenVO(
                authenticated: true,
                created: createdDate.ToString(DATE_FORMAT),
                expiration: expirationDate.ToString(DATE_FORMAT),
                accessToken: accessToken,
                refreshToken: refreshToken);
        }
    }
}
