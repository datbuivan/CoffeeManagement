using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoffeeManagement.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        Task<IDbContextTransaction> BeginTransactionAsync();
        IGenericRepository<TEntity> GenericRepository<TEntity>() where TEntity : BaseEntities;
        Task<int> Complete();
        Task RollbackAsync();
    }
}
