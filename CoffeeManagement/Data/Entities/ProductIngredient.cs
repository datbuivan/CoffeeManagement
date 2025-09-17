using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{

    // định mức
    public class ProductIngredient : BaseEntities
    {
        public Guid ProductId { get; set; }
        public Guid IngredientId { get; set; }
        [Precision(18, 4)]
        public decimal QuantityNeeded { get; set; }

        public Product Product { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}
