using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Models;

namespace CoffeeManagement.Interface
{
    public interface IOrderService
    {
        Task<OrderResultDto> GetById(Guid id);
        Task<object> CreateAndPayOrderAsync(OrderAndPayDto dto);
        Task HandleVnPayCallbackAsync(IQueryCollection responseData); // Cần cho VNPAY
    }
}
