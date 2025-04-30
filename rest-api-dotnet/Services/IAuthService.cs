using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Services
{
    public interface IAuthService
    {
        string GetGoogleLoginUrl(string state);
        Task<AuthResponseVO> ProcessGoogleCallbackAsync(string code, string state);
        Task<GoogleIdTokenPayloadVO> ValidateIdTokenWithGoogle(string idToken);
    }
}
