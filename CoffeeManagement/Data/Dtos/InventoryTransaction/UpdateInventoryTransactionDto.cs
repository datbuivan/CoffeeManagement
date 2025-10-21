namespace CoffeeManagement.DTOs.InventoryTransaction
{
    public class UpdateInventoryTransactionDto
    {
        public string TransactionType { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public Guid? RelatedDocumentId { get; set; }
    }
}