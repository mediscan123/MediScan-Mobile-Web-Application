using System.Linq;

namespace Tecknow.MediScan.Business.RepositoryPattern.Interfaces
{
    public interface IGenericRepository<T>
    {
        T GetById(int id);
        IQueryable<T> GetAll();
        void Update(T entity);
        void Add(T entity);
        void Delete(T entity);
    }
}