using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class Order : BaseEntities
    {
        public string UserId { get; set; } = null!; // IdentityUser.Id
        public int? TableNumber { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Processing, Served, Paid
        [Precision(18, 4)]
        public decimal TotalAmount { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    }
}
