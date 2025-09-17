using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class CategoryDto :BaseEntities
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
