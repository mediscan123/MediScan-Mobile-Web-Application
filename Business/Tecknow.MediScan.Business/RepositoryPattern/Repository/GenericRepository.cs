using System.Data.Entity;
using System.Linq;
using Tecknow.MediScan.Business.RepositoryPattern.Interfaces;

namespace Tecknow.MediScan.Business.RepositoryPattern.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbset;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbset;
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Add(T entity)
        {
            _dbset.Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
    }
}