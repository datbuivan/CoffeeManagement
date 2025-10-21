namespace CoffeeManagement.Data.Dtos.Shift
{

    // DTO để tạo/cập nhật ca
    public class ShiftCreateUpdateDto
    {
        public string Name { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
