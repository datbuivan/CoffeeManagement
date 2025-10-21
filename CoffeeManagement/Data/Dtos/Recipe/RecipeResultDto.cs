using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Dtos.Recipe
{
    public class RecipeResultDto : BaseEntities
    {
        public Guid ProductSizeId { get; set; }

        public Guid IngredientId { get; set; }

        public decimal QuantityUsed { get; set; }

    }
}