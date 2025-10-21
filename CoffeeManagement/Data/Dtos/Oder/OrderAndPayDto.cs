using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Data.Dtos.Order
{
    // DTO này chứa tất cả thông tin cần thiết để tạo đơn và thanh toán ngay
    public class OrderAndPayDto
    {
        [Required]
        public string UserId { get; set; }
        public Guid? TableId { get; set; } // Vẫn giữ để biết khách ngồi bàn nào

        [Required]
        [MinLength(1)]
        public List<OrderItemCreateDto> Items { get; set; } = new();

        // Thông tin thanh toán được gộp vào đây
        [Required]
        public string PaymentMethod { get; set; } // "Cash" hoặc "VnPay"

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; } = 0;
    }
}