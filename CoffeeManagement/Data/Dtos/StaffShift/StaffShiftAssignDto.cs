namespace CoffeeManagement.Data.Dtos.StaffSheet
{
    // DTO để nhận yêu cầu phân công ca
    public class StaffShiftAssignDto
    {
        public string StaffId { get; set; }
        public Guid ShiftId { get; set; }
        public DateOnly WorkDate { get; set; }
        public string? Notes { get; set; }
    }
}