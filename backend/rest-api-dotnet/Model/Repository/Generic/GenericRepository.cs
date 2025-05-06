using Microsoft.EntityFrameworkCore;
using RestApiDotNet.Model.Base;
using RestApiDotNet.Model.Context;
using RestApiDotNet.Repository;

namespace RestApiDotNet.Model.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MySQLContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(MySQLContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public List<T> FindAll()
        {
            return _dbSet.ToList();
        }

        public T FindByID(long id)
        {
            return _dbSet.SingleOrDefault(e => e.Id.Equals(id));
        }

        public T Create(T entity)
        {
            try
            {
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public T Update(T entity)
        {
            if (!Exists(entity.Id)) return null;

            var result = _dbSet.SingleOrDefault(p => p.Id == entity.Id);
            if (result == null) return null;

            try
            {
                _dbSet.Entry(result).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }
        public void Delete(long id)
        {
            var result = _dbSet.SingleOrDefault(p => p.Id == id);
            if (result == null) return;

            try
            {
                _dbSet.Remove(result);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool Exists(long id)
        {
            return _context.Books.Any(p => p.Id.Equals(id));
        }

        public List<T> FindWithPagedSearch(string query)
        {
            return _dbSet.FromSqlRaw(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }

            return int.Parse(result);
        }
    }
}
