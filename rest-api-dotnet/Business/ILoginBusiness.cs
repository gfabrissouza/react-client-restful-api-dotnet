using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Business
{
    public interface ILoginBusiness
    {
        TokenVO ValidateCredentials(UserVO user);
        TokenVO ValidadeCredentials(TokenVO token);
        bool RevokeToken(string userName);
    }
}
