using RestApiDotNet.Data.VO;
using RestApiDotNet.Model.Repository;

namespace RestApiDotNet.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IPersonRepository repository) 
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

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
