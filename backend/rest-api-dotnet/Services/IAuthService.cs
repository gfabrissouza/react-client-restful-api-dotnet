using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Services
{
    public interface IAuthService
    {
        string GetGoogleLoginUrl(string state);
        Task<AuthResponseVO> ProcessGoogleCallbackAsync(string code);
        Task<GoogleIdTokenPayloadVO> ValidateIdTokenWithGoogle(string idToken);
    }
}
