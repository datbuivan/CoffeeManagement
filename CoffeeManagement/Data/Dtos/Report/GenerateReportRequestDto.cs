using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Data.Dtos.Report
{
    public class GenerateReportRequestDto
    {
        [Required]
        public DateOnly ReportDate { get; set; }
    }
}