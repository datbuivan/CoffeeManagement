using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class Product : BaseEntities
    {
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [Precision(18, 4)]
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
        public Category Category { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new HashSet<ProductIngredient>();
        public ICollection<ProductSize> ProductSizes { get; set; } = new HashSet<ProductSize>();

    }
}
