using CoffeeManagement.Data;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Repositories
{
    public class ProductSizeRepository : IProductSizeRepository
    {
        private readonly DataContext _context;
        public ProductSizeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ProductSize> GetByIdAndProductId(Guid id, Guid productId)
        {
            return await _context.ProductSizes
                .SingleOrDefaultAsync(ps => ps.Id == id && ps.ProductId == productId);

        }

        public async Task<List<ProductSize>> GetByProductId(Guid productId)
        {
            return await _context.ProductSizes
                .Where(ps => ps.ProductId == productId)
                .ToListAsync();
        }
    }
}
