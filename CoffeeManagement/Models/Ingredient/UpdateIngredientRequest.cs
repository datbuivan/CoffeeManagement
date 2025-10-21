using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.Ingredient
{
    public class UpdateIngredientRequest
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Unit { get; set; } = null!;

        public decimal ReorderLevel { get; set; }
    }
}