using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class ProductSizeDto : BaseEntities
    {
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
