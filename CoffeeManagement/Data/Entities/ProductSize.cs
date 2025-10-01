using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class ProductSize : BaseEntities
    {
        public Guid ProductId { get; set; }
        public string Size { get; set; } = null!; // "S", "M", "L", ...
        [Precision(18, 4)]
        public decimal Price { get; set; }
        public Product Product { get; set; } = null!;
    }
}
