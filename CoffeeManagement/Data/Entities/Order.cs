using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class Order : BaseEntities
    {
        public string UserId { get; set; } = null!; 
        public Guid? TableId { get; set; } 
        public string Status { get; set; } = "Pending"; // Pending, Processing, Served, Paid, Cancelled

        [Precision(18, 4)]
        public decimal TotalAmount { get; set; } // Tổng tiền trước giảm giá
        [Precision(18, 4)]
        public decimal DiscountAmount { get; set; } = 0; // Số tiền giảm giá
        [Precision(18, 4)]
        public decimal FinalAmount { get; set; } // Tổng tiền sau giảm giá
        public Table? Table { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
