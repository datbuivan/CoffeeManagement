using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace CoffeeManagement.Interface
{
    public interface IOrderReportService
    {
        Task<(List<OrderReportDto> report, OrderReportSummaryDto summary, byte[]? pdfFile)>
            GetOrderReportAsync(DateTime fromDate, DateTime toDate, bool isExportPdf = false);
    }
}