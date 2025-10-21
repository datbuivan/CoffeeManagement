using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Order
{
    public class OrderResultDto : BaseEntities
    {
        public string Status { get; set; }
        public Guid? TableId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public List<OrderItemResultDto> OrderItems { get; set; } = new();
    }

}