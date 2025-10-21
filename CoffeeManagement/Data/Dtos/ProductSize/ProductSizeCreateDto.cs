namespace CoffeeManagement.Data.Dtos.ProductSize
{
    public class ProductSizeCreateDto
    {
        public Guid ProductId { get; set; }
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
    }
}