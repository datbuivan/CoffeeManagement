using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Order
{
    public class OrderItemResultDto : BaseEntities
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductSizeId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }

    }
}