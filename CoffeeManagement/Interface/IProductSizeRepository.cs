using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Interface
{
    public interface IProductSizeRepository
    {
        Task<List<ProductSize>> GetByProductId(Guid productId);
        Task<ProductSize> GetByIdAndProductId(Guid id,Guid productId);
    }
}
