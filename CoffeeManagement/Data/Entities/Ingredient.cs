using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{

    // Nguyên Liệu
    public class Ingredient : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!; // Ví dụ: kg, lít, gram, ml

        [Precision(18, 4)]
        public decimal CurrentStock { get; set; } // Tồn kho hiện tại
        [Precision(18, 4)]
        public decimal ReorderLevel { get; set; } // Mức đặt hàng lại
        public bool IsActive { get; set; } = true; // Ngừng sử dụng nguyên liệu

        public ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new HashSet<InventoryTransaction>();
    }
}
