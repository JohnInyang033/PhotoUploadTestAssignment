using BandLabTestAssignment.Data;
using BandLabTestAssignment.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BandLabTestAssignment.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(Guid? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public bool DoesExist(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }
    }
}
