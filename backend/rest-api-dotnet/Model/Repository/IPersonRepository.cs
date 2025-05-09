﻿using RestApiDotNet.Repository;

namespace RestApiDotNet.Model.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string lastName);
    }
}
