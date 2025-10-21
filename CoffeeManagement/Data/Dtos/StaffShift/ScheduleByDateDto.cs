namespace CoffeeManagement.Data.Dtos.StaffSheet
{
    // DTO này nhóm các ca làm việc theo từng ngày
    public class ScheduleByDateDto
    {
        public DateOnly Date { get; set; }
        public List<StaffShiftResultDto> Assignments { get; set; } = new List<StaffShiftResultDto>();
    }
}