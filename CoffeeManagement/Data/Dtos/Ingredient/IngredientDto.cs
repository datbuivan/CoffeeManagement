using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class IngredientResultDto : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal CurrentStock { get; set; }
        public decimal ReorderLevel { get; set; }
    }
}
