using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Category
{
    public class CategoryResultDto : BaseEntities
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}