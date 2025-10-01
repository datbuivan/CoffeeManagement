using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Table
{
    public class TableResultDto : BaseEntities
    {
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "Available";
    }
}