using RestApiDotNet.Model.Base;

namespace RestApiDotNet.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T entity);
        T FindByID(long id);
        List<T> FindAll();
        T Update(T entity);
        void Delete(long id);
        List<T> FindWithPagedSearch(string query);
        int GetCount(string query);
    }
}
