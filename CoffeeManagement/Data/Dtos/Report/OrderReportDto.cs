using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Report
{
    public class OrderReportDto : BaseEntities
    {
        public string UserId { get; set; } = null!;
        public string? UserName { get; set; }
        public string? TableName { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}