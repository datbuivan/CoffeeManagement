namespace CoffeeManagement.Data.Dtos.Report
{
    public class OrderReportResponseDto
    {
        public List<OrderReportDto> Report { get; set; } = new();
        public OrderReportSummaryDto Summary { get; set; } = new();
    }
}