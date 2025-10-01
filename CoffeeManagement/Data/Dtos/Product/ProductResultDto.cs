using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Product
{
    public class ProductResultDto : BaseEntities
    {
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
    }
}
