using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{

    // Nguyên Liệu
    public class Ingredient : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        [Precision(18, 4)]
        public decimal CurrentStock { get; set; }
        [Precision(18, 4)]
        public decimal ReorderLevel { get; set; }

        public ICollection<ProductIngredient> ProductIngredients { get; set; } = new HashSet<ProductIngredient>();
        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new HashSet<InventoryTransaction>();
    }
}
