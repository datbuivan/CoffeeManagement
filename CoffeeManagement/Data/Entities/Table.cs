using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Entities
{
    public class Table : BaseEntities
    {
        public string Name { get; set; } = null!;

        public string Status { get; set; } = "Available"; // Available, Occupied, Cleaning
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
