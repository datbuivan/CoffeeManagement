using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Models.Ingredient
{
    public class CreateIngredientRequest
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(20)]
        public string Unit { get; set; } = null!;

        public decimal CurrentStock { get; set; } = 0;

        public decimal ReorderLevel { get; set; } = 0;
    }
}