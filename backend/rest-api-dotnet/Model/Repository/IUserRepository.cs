using RestApiDotNet.Data.VO;

namespace RestApiDotNet.Model.Repository
{
    public interface IUserRepository
    {
        User? ValidateCredentials(UserVO user);
        User? ValidateCredentials(string userName);
        bool RevokeToken(string userName);
        User? RefreshUserInfo(User user);
        User? GetUserByEmail(string email);
    }
}
