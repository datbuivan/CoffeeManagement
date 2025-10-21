using CoffeeManagement.Data.Dtos.Report;

namespace CoffeeManagement.Interface
{
    public interface IProductTopSaleService
    {
        Task<IEnumerable<TopSellingProductDto>> GetTopProductsAsync(string period);
    }
}
