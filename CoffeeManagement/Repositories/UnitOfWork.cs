using CoffeeManagement.Data;
using CoffeeManagement.Data.Entities.Custom;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace CoffeeManagement.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private Hashtable _repositories = new Hashtable();
        public UnitOfWork(DataContext context)
        {
            _context = context;

        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntities
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                if (repositoryInstance == null)
                    throw new InvalidOperationException($"Could not create repository instance for {type}.");

                _repositories.Add(type, repositoryInstance);
            }

            return _repositories[type] as IGenericRepository<TEntity>
                ?? throw new InvalidOperationException($"Repository instance for {type} is not of expected type.");
        }

        public async Task RollbackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }


    }
}
