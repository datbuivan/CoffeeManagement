using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class OrderItem : BaseEntities
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ProductSizeId { get; set; }
        public int Quantity { get; set; }

        [Precision(18, 4)]
        public decimal UnitPrice { get; set; } // Giá tại thời điểm bán
        [Precision(18, 4)]
        public decimal SubTotal { get; set; }

        public Order Order { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public ProductSize? ProductSize { get; set; }
    }
}
