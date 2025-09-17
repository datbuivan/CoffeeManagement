using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class OrderDto : BaseEntities
    {
        public string UserId { get; set; } = null!;   // IdentityUser.Id
        public int? TableNumber { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Processing, Served, Paid
        public decimal TotalAmount { get; set; }
    }
}
