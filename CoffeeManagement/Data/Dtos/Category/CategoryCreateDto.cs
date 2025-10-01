namespace CoffeeManagement.Data.Dtos.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}