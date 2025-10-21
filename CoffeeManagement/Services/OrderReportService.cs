using CoffeeManagement.Data.Dtos.Report;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.EntityFrameworkCore;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using PdfTable = iText.Layout.Element.Table;
using System.IO;

namespace CoffeeManagement.Services
{
    public class OrderReportService : IOrderReportService
    {
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly ILogger<OrderReportService> _logger;

        public OrderReportService(
            IGenericRepository<Order> orderRepo,
            ILogger<OrderReportService> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }

        public async Task<(List<OrderReportDto> report, OrderReportSummaryDto summary, byte[]? pdfFile)>
            GetOrderReportAsync(DateTime fromDate, DateTime toDate, bool isExportPdf = false)
        {
            try
            {
                if (fromDate > toDate)
                {
                    throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc");
                }

                var startDate = fromDate.Date;
                var endDate = toDate.Date.AddDays(1).AddTicks(-1);

                var orders = await _orderRepo.FindAllAsync(
                    o => o.CreatedAt >= startDate && o.CreatedAt <= endDate,
                    include: q => q.Include(o => o.Table).Include(o => o.User).AsNoTracking()
                );

                var report = orders
                    .OrderByDescending(o => o.CreatedAt)
                    .Select(o => new OrderReportDto
                    {
                        Id = o.Id,
                        CreatedAt = o.CreatedAt,
                        UpdatedAt = o.UpdatedAt,
                        UserId = o.UserId,
                        UserName = o.User?.FullName ?? o.User?.UserName ?? "N/A",
                        TableName = o.Table?.Name ?? "N/A",
                        Status = o.Status,
                        TotalAmount = o.TotalAmount,
                        DiscountAmount = o.DiscountAmount,
                        FinalAmount = o.FinalAmount
                    }).ToList();

                var paidOrders = report.Where(o => o.Status == "Paid").ToList();
                var summary = new OrderReportSummaryDto
                {
                    TotalOrders = report.Count,
                    TotalRevenue = paidOrders.Sum(o => o.TotalAmount),
                    TotalDiscount = paidOrders.Sum(o => o.DiscountAmount),
                    NetRevenue = paidOrders.Sum(o => o.FinalAmount)
                };

                byte[]? pdfBytes = null;
                if (isExportPdf)
                {
                    pdfBytes = await GeneratePdfReportAsync(report, summary, fromDate, toDate);
                }

                return (report, summary, pdfBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating order report from {FromDate} to {ToDate}", fromDate, toDate);
                throw;
            }
        }

        private async Task<byte[]> GeneratePdfReportAsync(
            List<OrderReportDto> report,
            OrderReportSummaryDto summary,
            DateTime fromDate,
            DateTime toDate)
        {
            return await Task.Run(() =>
            {
                using var ms = new MemoryStream();
                using var writer = new PdfWriter(ms);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                PdfFont font;
                try
                {
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Could not load custom font, using default");
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                }

                // ===== TIÊU ĐỀ =====
                var title = new Paragraph($"BAO CAO DON HANG TU {fromDate:dd/MM/yyyy} DEN {toDate:dd/MM/yyyy}")
                    .SetFont(font)
                    .SetFontSize(16)
                    .SetBold()
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);
                document.Add(new Paragraph("\n").SetFont(font));

                // ===== BẢNG DỮ LIỆU =====
                var columnWidths = new float[] { 1.5f, 2f, 1.5f, 1.5f, 2f, 2f, 2f };
                var table = new PdfTable(UnitValue.CreatePercentArray(columnWidths))
                    .UseAllAvailableWidth();

                // Headers
                string[] headers = { "Ma don", "Nguoi tao", "Ban", "Trang thai", "Tong tien", "Giam gia", "Thuc thu" };
                foreach (var header in headers)
                {
                    var cell = new Cell()
                        .Add(new Paragraph(header).SetFont(font).SetBold())
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY)
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddHeaderCell(cell);
                }

                foreach (var o in report)
                {
                    table.AddCell(CreateCell(o.Id.ToString(), font));
                    table.AddCell(CreateCell(o.UserId ?? "N/A", font));
                    table.AddCell(CreateCell(o.TableName ?? "N/A", font));
                    table.AddCell(CreateCell(o.Status ?? "", font));
                    table.AddCell(CreateCell(o.TotalAmount.ToString("N0"), font, TextAlignment.RIGHT));
                    table.AddCell(CreateCell(o.DiscountAmount.ToString("N0"), font, TextAlignment.RIGHT));
                    table.AddCell(CreateCell(o.FinalAmount.ToString("N0"), font, TextAlignment.RIGHT));
                }

                document.Add(table);
                document.Add(new Paragraph("\n").SetFont(font));

                document.Add(new Paragraph($"Tong don: {summary.TotalOrders}").SetFont(font).SetBold());
                document.Add(new Paragraph($"Tong doanh thu: {summary.TotalRevenue:N0} VND").SetFont(font).SetBold());
                document.Add(new Paragraph($"Tong giam gia: {summary.TotalDiscount:N0} VND").SetFont(font).SetBold());
                document.Add(new Paragraph($"Doanh thu thuc: {summary.NetRevenue:N0} VND").SetFont(font).SetBold().SetFontSize(12));

                document.Add(new Paragraph($"\nNgay xuat bao cao: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetFont(font)
                    .SetFontSize(9)
                    .SetTextAlignment(TextAlignment.RIGHT));

                document.Close();
                return ms.ToArray();
            });
        }

        private Cell CreateCell(string text, PdfFont font, TextAlignment alignment = TextAlignment.LEFT)
        {
            return new Cell()
                .Add(new Paragraph(text).SetFont(font).SetFontSize(10))
                .SetTextAlignment(alignment)
                .SetPadding(5);
        }
    }
}