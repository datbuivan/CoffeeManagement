using System.ComponentModel.DataAnnotations;

namespace CoffeeManagement.Data.Dtos.Order
{
    public class OrderItemCreateDto
    {
        [Required]
        public Guid ProductSizeId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}