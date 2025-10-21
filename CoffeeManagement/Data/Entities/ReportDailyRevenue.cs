using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    public class ReportDailyRevenue : BaseEntities
    {
        public DateTime ReportDate { get; set; }   // Ngày báo cáo
        [Precision(18, 4)]
        public decimal TotalRevenue { get; set; }  // Tổng doanh thu
        public int TotalOrders { get; set; }       // Tổng số đơn
        public int TotalProductsSold { get; set; } // Số lượng sản phẩm bán
    }
}
