using RestApiDotNet.Model;

namespace RestApiDotNet.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO person);
        BookVO FindByID(long id);
        List<BookVO> FindAll();
        BookVO Update(BookVO person);
        void Delete(long id);
    }
}
