using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{

    // Nhập xuất kho
    public class InventoryTransaction : BaseEntities
    {
        public Guid IngredientId { get; set; } // Id nguyên liệu
        public string TransactionType { get; set; } = null!; // IN / OUT / ADJUSTMENT / LOSS
        public decimal Quantity { get; set; } // số lượng
        [Precision(18, 4)]
        public decimal? UnitPrice { get; set; } // giá
        public string UserId { get; set; } = null!;
        // ID của đơn hàng (nếu là xuất kho), hoặc ID của đơn mua hàng (nếu là nhập kho)
        public Guid? RelatedDocumentId { get; set; }

        public Ingredient Ingredient { get; set; } = null!;
    }
}
