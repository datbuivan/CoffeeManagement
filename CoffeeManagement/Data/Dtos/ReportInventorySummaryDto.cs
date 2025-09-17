using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos
{
    public class ReportInventorySummaryDto : BaseEntities
    {
        public DateTime ReportDate { get; set; }
        public Guid IngredientId { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal InQuantity { get; set; }
        public decimal OutQuantity { get; set; }
        public decimal ClosingStock { get; set; }
    }
}
