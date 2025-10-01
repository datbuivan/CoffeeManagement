namespace CoffeeManagement.Data.Dtos.Product
{
    public class ProductCreateDto
    {
        public string Name { get; set; } = null!;
        public Guid CategoryId { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
