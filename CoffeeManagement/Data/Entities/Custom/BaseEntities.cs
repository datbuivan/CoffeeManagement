namespace CoffeeManagement.Data.Entities.Custom
{
    public class BaseEntities
    {
        public Guid Id { get; set; }
        public DateTime? CreatedAt { get; set; } = default(DateTime?);
        public DateTime? UpdatedAt { get; set; }
    }
}
