using CoffeeManagement.Data.Dtos.Order;
using CoffeeManagement.Data.Entities;

namespace CoffeeManagement.Interface
{
    public interface IOrderItemService
    {
        Task<IReadOnlyList<OrderItemResultDto>> GetPendingOrderItemsAsync();
        Task<bool> UpdateIsDeliverOrderAsync(Guid orderItemId, bool isDeliver);
        Task<bool> UpdateIsDeliverOrdersAsync(IEnumerable<Guid> orderItemIds, bool isDeliver);
    }
}
