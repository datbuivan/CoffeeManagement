using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Entities
{
    public class Category : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
