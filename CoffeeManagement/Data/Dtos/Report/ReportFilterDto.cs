namespace CoffeeManagement.Data.Dtos.Report
{
    public class ReportFilterDto
    {
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsExport { get; set; } = false;
    }
}