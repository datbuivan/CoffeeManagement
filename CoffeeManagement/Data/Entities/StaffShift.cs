using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Entities
{
    public class StaffShift : BaseEntities
    {
        public string StaffId { get; set; } // FK tới User/Employee
        public Guid ShiftId { get; set; } // FK tới Shift

        // Sử dụng DateOnly để chỉ lưu ngày, không cần thời gian
        public DateOnly WorkDate { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public ApplicationUser Staff { get; set; } = null!; // Giả sử bạn có entity User
        public Shift Shift { get; set; } = null!;
    }
}