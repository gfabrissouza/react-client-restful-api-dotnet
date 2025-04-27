using RestApiDotNet.Data.VO;
using RestApiDotNet.Model;
using RestApiDotNet.Repository;

namespace RestApiDotNet.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IRepository<Person> _repository;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IRepository<Person> repository) 
        {
            this._repository = repository;
            this._converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            return _converter.ParseList(_repository.FindAll());
        }

        public PersonVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public PersonVO Create(PersonVO person)
        {
            var entity = _converter.Parse(person);
            return _converter.Parse(_repository.Create(entity));
        }

        public PersonVO Update(PersonVO person)
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
