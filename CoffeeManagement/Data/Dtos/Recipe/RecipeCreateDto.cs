

namespace CoffeeManagement.Data.Dtos.Recipe
{
    public class RecipeCreateDto
    {
        public Guid ProductSizeId { get; set; }

        public Guid IngredientId { get; set; }

        public decimal QuantityUsed { get; set; }

    }
}