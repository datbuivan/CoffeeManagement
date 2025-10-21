using CoffeeManagement.Data;
using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Services
{
    public class ProductTopSaleService : IProductTopSaleService
    {
        private readonly IGenericRepository<OrderItem> _orderItemRepository;
        private readonly DataContext _context;

        public ProductTopSaleService(IGenericRepository<OrderItem> orderItemRepository, DataContext context)
        {
            _orderItemRepository = orderItemRepository;
            _context = context;
        }

        public async Task<IEnumerable<TopSellingProductDto>> GetTopProductsAsync(string period)
        {
            DateTime startDate, endDate = DateTime.Now;

            switch (period.ToLower())
            {
                case "today":
                    startDate = DateTime.Today;
                    break;
                case "week":
                    int diff = (7 + (int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday) % 7;
                    startDate = DateTime.Today.AddDays(-1 * diff);
                    break;
                case "month":
                    startDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    break;
                case "year":
                    startDate = new DateTime(DateTime.Today.Year, 1, 1);
                    break;
                default:
                    throw new ArgumentException("Invalid period. Use 'today', 'week', 'month', or 'year'.");
            }

            var topProducts = await _context.OrderItems
                .Include(x => x.Product)
                .Include(x => x.Order)
                .Where(x => x.Order.CreatedAt >= startDate
                            && x.Order.CreatedAt <= endDate
                            && x.Order.Status == "Paid") // Chỉ lấy hoá đơn đã thanh toán
                .GroupBy(x => new { x.ProductId, x.Product.Name })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    TotalQuantity = g.Sum(i => i.Quantity),
                    TotalRevenue = g.Sum(i => i.SubTotal)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(10)
                .ToListAsync();

            return topProducts;
        }
    }
}
