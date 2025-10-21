using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.ProductSize
{
    public class ProductSizeResultDto : BaseEntities
    {
        public Guid ProductId { get; set; }
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
