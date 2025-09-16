using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{

    // Nhập xuất kho
    public class InventoryTransaction : BaseEntities
    {
        public Guid IngredientId { get; set; }
        public string TransactionType { get; set; } = null!; // IN / OUT / ADJUSTMENT
        [Precision(18, 4)]
        public decimal Quantity { get; set; }
        [Precision(18, 4)]
        public decimal? UnitPrice { get; set; }
        public Guid? OrderId { get; set; } // Nếu liên quan tới order

        public Ingredient Ingredient { get; set; } = null!;
    }
}
