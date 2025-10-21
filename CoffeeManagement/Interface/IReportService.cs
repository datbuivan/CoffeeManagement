using CoffeeManagement.Data.Dtos.Report;

namespace CoffeeManagement.Interface
{
    public interface IReportService
    {
        Task<ReportDailyRevenueDto> GenerateDailyReportAsync(DateOnly reportDate);

        Task<IEnumerable<ReportDailyRevenueDto>> GetReportsAsync(ReportFilterDto filter);
        // byte[] ExportReportsToExcel(IEnumerable<ReportDailyRevenueDto> reports);
    }
}