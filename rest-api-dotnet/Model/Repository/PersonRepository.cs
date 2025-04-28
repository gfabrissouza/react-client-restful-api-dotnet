using RestApiDotNet.Model.Context;
using RestApiDotNet.Model.Repository.Generic;

namespace RestApiDotNet.Model.Repository
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MySQLContext context) : base(context)
        {
        }

        public Person Disable(long id)
        {
            if (!_context.People.Any(p => p.Id.Equals(id))) return null;

            var user = _context.People.SingleOrDefault(p => p.Id.Equals(id));

            if (user != null) {
                user.Enabled = false;
                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return user;
        }

        public List<Person> FindByName(string firstName, string lastName)
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.People
                    .Where(p => p.FirstName.Contains(firstName) && p.LastName.Contains(lastName))
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(firstName))
            {
                return _context.People
                    .Where(p => p.FirstName.Contains(firstName))
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(lastName))
            {
                return _context.People
                    .Where(p => p.LastName.Contains(lastName))
                    .ToList();
            }

            return null;
        }
    }
}
