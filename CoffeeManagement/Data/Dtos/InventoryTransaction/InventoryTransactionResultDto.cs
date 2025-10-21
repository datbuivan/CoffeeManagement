using CoffeeManagement.Data.Entities;
using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.DTOs.InventoryTransaction
{
    // DTO cho kết quả trả về
    public class InventoryTransactionResultDto : BaseEntities
    {
        public Guid IngredientId { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public string UserId { get; set; } = null!;
        public Guid? RelatedDocumentId { get; set; }
    }
}