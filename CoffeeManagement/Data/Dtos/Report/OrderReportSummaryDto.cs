namespace CoffeeManagement.Data.Dtos.Report
{
    public class OrderReportSummaryDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetRevenue { get; set; }
    }
}
