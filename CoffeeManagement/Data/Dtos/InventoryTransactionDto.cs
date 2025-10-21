using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class InventoryTransactionDto : BaseEntities
    {
        public Guid IngredientId { get; set; }
        public string TransactionType { get; set; } = null!;
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public Guid? OrderId { get; set; }
    }
}
