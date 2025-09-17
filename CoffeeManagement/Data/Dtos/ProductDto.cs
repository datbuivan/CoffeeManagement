using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class ProductDto : BaseEntities
    {
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
