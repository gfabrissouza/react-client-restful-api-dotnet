using RestApiDotNet.Data.Converter.Contract;
using RestApiDotNet.Model;

namespace RestApiDotNet.Data.VO
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        public Person Parse(PersonVO origin)
        {
            if (origin == null) return null;

            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Address = origin.Address,
                Gender = origin.Gender
            };
        }

        public PersonVO Parse(Person destination)
        {
            if (destination == null) return null;

            return new PersonVO
            {
                Id = destination.Id,
                FirstName = destination.FirstName,
                LastName = destination.LastName,
                Address = destination.Address,
                Gender = destination.Gender
            };
        }

        public List<Person> ParseList(List<PersonVO> origin)
        {
            return origin.Select(item => Parse(item)).ToList();
        }

        public List<PersonVO> ParseList(List<Person> destination)
        {
            return destination.Select(item => Parse(item)).ToList();
        }
    }
}
