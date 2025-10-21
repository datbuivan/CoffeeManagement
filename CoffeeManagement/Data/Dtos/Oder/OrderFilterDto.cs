namespace CoffeeManagement.Data.Dtos.Order
{
    public class OrderFilterDto
    {
        public string? Status { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}