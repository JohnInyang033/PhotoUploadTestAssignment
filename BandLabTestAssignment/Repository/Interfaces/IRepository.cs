using System.Linq.Expressions;

namespace BandLabTestAssignment.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetAsync(Guid? id);
        Task<List<T>> GetAllAsync();
        void Add(T entity);
        void Remove(T entity); 
        Task<int> SaveChangesAsync(); 
        bool DoesExist(Expression<Func<T, bool>> predicate);
    }
}
