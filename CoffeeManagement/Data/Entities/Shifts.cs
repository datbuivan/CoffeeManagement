using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Entities
{
    public class Shift : BaseEntities
    {
        public string Name { get; set; } = null!;

        // Sử dụng TimeOnly để lưu trữ giờ và phút, không cần ngày
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public bool IsActive { get; set; } = true;

        // Một ca có thể có nhiều lịch phân công
        public ICollection<StaffShift> EmployeeShifts { get; set; } = new HashSet<StaffShift>();
    }
}