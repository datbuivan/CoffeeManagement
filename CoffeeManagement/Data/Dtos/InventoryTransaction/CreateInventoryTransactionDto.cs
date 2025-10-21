using System.ComponentModel.DataAnnotations;
using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.DTOs.InventoryTransaction
{
    public class CreateInventoryTransactionDto
    {
        public Guid IngredientId { get; set; }

        public string TransactionType { get; set; } = null!;

        public decimal Quantity { get; set; }

        public decimal? UnitPrice { get; set; }

        public string UserId { get; set; } = null!;

        public Guid? RelatedDocumentId { get; set; }
    }
}