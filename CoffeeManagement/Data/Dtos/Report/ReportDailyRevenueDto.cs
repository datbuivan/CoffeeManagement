using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Report
{
    public class ReportDailyRevenueDto
    {
        public Guid Id { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProductsSold { get; set; }
    }
}
