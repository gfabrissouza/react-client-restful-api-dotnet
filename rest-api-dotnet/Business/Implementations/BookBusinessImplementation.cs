using RestApiDotNet.Data.VO;
using RestApiDotNet.Model;
using RestApiDotNet.Repository;

namespace RestApiDotNet.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BookConverter _converter;

        public BookBusinessImplementation(IRepository<Book> repository) 
        {
            this._repository = repository;
            this._converter = new BookConverter();
        }

        public List<BookVO> FindAll()
        {
            return _converter.ParseList(_repository.FindAll());
        }

        public BookVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public BookVO Create(BookVO person)
        {
            var entity = _converter.Parse(person);
            return _converter.Parse(_repository.Create(entity));
        }

        public BookVO Update(BookVO person)
        {
            var entity = _converter.Parse(person);
            return _converter.Parse(_repository.Update(entity));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
