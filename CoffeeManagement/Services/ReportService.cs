using System.Drawing;
using AutoMapper;
using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace CoffeeManagement.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<ReportDailyRevenueDto> GenerateDailyReportAsync(DateOnly reportDate)
        {
            // Bắt đầu transaction để đảm bảo toàn vẹn dữ liệu
            await using var transaction = await _uow.BeginTransactionAsync();
            try
            {
                var startOfDay = reportDate.ToDateTime(TimeOnly.MinValue);
                var endOfDay = startOfDay.AddDays(1);

                // Lấy tất cả các order đã thanh toán trong ngày
                var ordersInDay = await _uow.GenericRepository<Order>().FindAllAsync(
                    predicate: o => o.Status == "Paid" && o.CreatedAt >= startOfDay && o.CreatedAt < endOfDay,
                    include: q => q.Include(o => o.OrderItems)
                );

                // Tính toán các chỉ số
                var totalRevenue = ordersInDay.Sum(o => o.FinalAmount);
                var totalOrders = ordersInDay.Count;
                var totalProductsSold = ordersInDay.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity);

                // Kiểm tra xem báo cáo cho ngày này đã tồn tại chưa
                var existingReport = await _uow.GenericRepository<ReportDailyRevenue>()
                    .FindSingleAsync(r => r.ReportDate.Date == startOfDay.Date);

                if (existingReport != null)
                {
                    // Nếu đã có, cập nhật lại
                    existingReport.TotalRevenue = totalRevenue;
                    existingReport.TotalOrders = totalOrders;
                    existingReport.TotalProductsSold = totalProductsSold;
                    existingReport.UpdatedAt = DateTime.UtcNow;
                    _uow.GenericRepository<ReportDailyRevenue>().Update(existingReport);
                }
                else
                {
                    // Nếu chưa có, tạo mới
                    existingReport = new ReportDailyRevenue
                    {
                        ReportDate = startOfDay,
                        TotalRevenue = totalRevenue,
                        TotalOrders = totalOrders,
                        TotalProductsSold = totalProductsSold
                    };
                    _uow.GenericRepository<ReportDailyRevenue>().Add(existingReport);
                }

                await _uow.Complete();
                await transaction.CommitAsync();

                return _mapper.Map<ReportDailyRevenueDto>(existingReport);
            }
            catch (Exception)
            {
                await _uow.RollbackAsync();
                throw; // Ném lại lỗi để controller có thể xử lý
            }
        }

        public async Task<IEnumerable<ReportDailyRevenueDto>> GetReportsAsync(ReportFilterDto filter)
        {
            var reports = await _uow.GenericRepository<ReportDailyRevenue>().FindAllAsync(
                predicate: r =>
                    (!filter.StartDate.HasValue || r.ReportDate.Date >= filter.StartDate.Value.ToDateTime(TimeOnly.MinValue).Date) &&
                    (!filter.EndDate.HasValue || r.ReportDate.Date <= filter.EndDate.Value.ToDateTime(TimeOnly.MinValue).Date)
            );

            return _mapper.Map<IEnumerable<ReportDailyRevenueDto>>(reports.OrderByDescending(r => r.ReportDate));
        }

        // public byte[] ExportReportsToExcel(IEnumerable<ReportDailyRevenueDto> reports)
        // {
        //     // Cài đặt License context cho EPPlus
        //     ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //     using (var package = new ExcelPackage())
        //     {
        //         var worksheet = package.Workbook.Worksheets.Add("Daily Revenue Report");

        //         // --- Tạo Header ---
        //         worksheet.Cells[1, 1].Value = "Report Date";
        //         worksheet.Cells[1, 2].Value = "Total Revenue";
        //         worksheet.Cells[1, 3].Value = "Total Orders";
        //         worksheet.Cells[1, 4].Value = "Total Products Sold";

        //         // Định dạng Header
        //         using (var range = worksheet.Cells[1, 1, 1, 4])
        //         {
        //             range.Style.Font.Bold = true;
        //             range.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //             range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        //             range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //         }

        //         // --- Đổ dữ liệu ---
        //         int row = 2;
        //         foreach (var report in reports)
        //         {
        //             worksheet.Cells[row, 1].Value = report.ReportDate.ToString("yyyy-MM-dd");
        //             worksheet.Cells[row, 2].Value = report.TotalRevenue;
        //             worksheet.Cells[row, 3].Value = report.TotalOrders;
        //             worksheet.Cells[row, 4].Value = report.TotalProductsSold;
        //             row++;
        //         }

        //         // --- Định dạng các cột ---
        //         worksheet.Cells["A:A"].Style.Numberformat.Format = "yyyy-mm-dd";
        //         worksheet.Cells["B:B"].Style.Numberformat.Format = "#,##0";
        //         worksheet.Cells["C:C"].Style.Numberformat.Format = "#,##0";
        //         worksheet.Cells["D:D"].Style.Numberformat.Format = "#,##0";

        //         // Tự động giãn cột
        //         worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

        //         return package.GetAsByteArray();
        //     }
        // }
    }
}