using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.Shift
{
    // DTO để hiển thị thông tin ca
    public class ShiftResultDto : BaseEntities
    {
        public string Name { get; set; }
        public string StartTime { get; set; } // Dạng string "HH:mm"
        public string EndTime { get; set; } // Dạng string "HH:mm"
        public bool IsActive { get; set; }
    }
}