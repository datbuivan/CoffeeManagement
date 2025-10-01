using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Models;

namespace CoffeeManagement.Interface
{
    public interface IOrderService
    {
        Task<OrderDto> Create(string userId, int? tableNumber, List<(Guid productId, Guid ProductSizeId, int quantity)> items);
        Task<OrderDto?> GetById(Guid orderId);
        Task<IEnumerable<OrderDto>> Get();
        Task<OrderDto?> UpdateOrderStatus(Guid orderId, string status);
        Task<string> CreatePaymentUrl(Guid orderId, string clientIpAddr, string returnUrl);
        Task<bool> ProcessPaymentResponse(VnPayResponseDto response);
    }
}
