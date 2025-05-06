using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidadeCredentials(TokenVO token);
        bool RevokeToken(string userName);
        TokenVO ValidateUserByEmail(string email);
        string GetGoogleLoginUrl(string state);
        Task<AuthResponseVO> ProcessGoogleCallbackAsync(string code);
        Task<GoogleIdTokenPayloadVO> ValidateIdTokenWithGoogle(string idToken);
    }
}
