using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoffeeManagement.Data.Entities
{
    // tồn kho theo ngày
    public class ReportInventorySummary : BaseEntities
    {
        public DateTime ReportDate { get; set; }   // Ngày báo cáo
        public Guid IngredientId { get; set; }     // Nguyên liệu
        [Precision(18, 4)]
        public decimal OpeningStock { get; set; }  // Tồn đầu ngày
        [Precision(18, 4)]
        public decimal InQuantity { get; set; }    // Nhập trong ngày
        [Precision(18, 4)]
        public decimal OutQuantity { get; set; }   // Xuất trong ngày
        [Precision(18, 4)]
        public decimal ClosingStock { get; set; }  // Tồn cuối ngày

        public Ingredient Ingredient { get; set; } = null!;
    }
}
