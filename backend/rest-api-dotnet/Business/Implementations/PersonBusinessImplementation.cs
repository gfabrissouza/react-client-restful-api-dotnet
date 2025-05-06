using RestApiDotNet.Data.VO;
using RestApiDotNet.HyperMedia.Utils;
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

        public List<PersonVO> FindByName(string firstName, string lastName)
        {
            return _converter.ParseList(_repository.FindByName(firstName, lastName));
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var sort = (!string.IsNullOrEmpty(sortDirection) && !sortDirection.Equals("desc") ? "asc" : "desc");
            var size = pageSize < 1 ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            var query = @"select * from person p where 1 = 1";
            if (!string.IsNullOrEmpty(name)) query = query + $" and p.first_name like '%{name}%' ";
            query = query + $" order by p.first_name {sort} limit {size} offset {offset}";
            
            string countQuery = @"select count(*) from person p where 1 = 1";
            if (!string.IsNullOrEmpty(name)) countQuery = countQuery + $" and p.first_name like '%{name}%' ";


            var person = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);
            
            return new PagedSearchVO<PersonVO> 
            { 
                CurrentPage = page,
                List = _converter.ParseList(person),
                PageSize = size, 
                SortDirections = sort, 
                TotalResults = totalResults,
            };
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
