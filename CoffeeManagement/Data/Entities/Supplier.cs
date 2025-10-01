using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Entities
{
    public class Supplier : BaseEntities
    {
        public string Name { get; set; } = null!;
        public string ContactName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}
