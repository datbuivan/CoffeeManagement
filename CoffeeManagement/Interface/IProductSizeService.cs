using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoffeeManagement.Interface
{
    public interface IProductSizeService
    {
        Task<IEnumerable<ProductSizeResultDto>> GetByProductIdAsync(Guid productId);
    }
}
