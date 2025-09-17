using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class ProductIngredientDto : BaseEntities
    {
        public Guid ProductId { get; set; }
        public Guid IngredientId { get; set; }
        public decimal QuantityNeeded { get; set; }
    }
}
