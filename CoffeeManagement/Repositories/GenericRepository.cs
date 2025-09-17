using CoffeeManagement.Data;
using CoffeeManagement.Data.Entities.Custom;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoffeeManagement.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntities
    {
        private readonly DataContext _context;
        public GenericRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }
        public void RemoveRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
       
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>>? include = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(predicate)
                ?? throw new InvalidOperationException("Entity not found.");
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id)
                ?? throw new InvalidOperationException("Entity not found.");
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        //public async Task<int> CountAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).CountAsync();
        //}


        //public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).FirstOrDefaultAsync()
        //        ?? throw new InvalidOperationException("Entity not found.");
        //}

        //public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        //{
        //    return await ApplySpecification(spec).ToListAsync();
        //}
        //private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        //{
        //    return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        //}
    }
}
