using CoffeeManagement.Data.Entities.Custom;

namespace CoffeeManagement.Data.Dtos.StaffSheet
{
    public class StaffInfo
    {
        public string Id { get; set; }
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }

}