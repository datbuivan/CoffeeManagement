using CoffeeManagement.Data.Dtos.Shift;
using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.StaffSheet
{
    // DTO để nhận yêu cầu phân công ca
    public class StaffShiftResultDto : BaseEntities
    {
        public string StaffId { get; set; } // FK tới User/Employee
        public Guid ShiftId { get; set; } // FK tới Shift
        public DateOnly WorkDate { get; set; }
        public string? Notes { get; set; }
        public ShiftResultDto Shift { get; set; } // Thông tin ca
        public StaffInfo Staff { get; set; } // Thông tin nhân viên
    }
}