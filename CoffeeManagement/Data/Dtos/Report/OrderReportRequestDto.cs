using System.ComponentModel.DataAnnotations;
namespace CoffeeManagement.Data.Dtos.Report
{
    public class OrderReportRequestDto
    {
        [Required(ErrorMessage = "Ngày bắt đầu là bắt buộc")]
        public DateTime FromDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc là bắt buộc")]
        public DateTime ToDate { get; set; }
    }
}