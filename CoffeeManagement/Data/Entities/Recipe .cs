using CoffeeManagement.Data.Entities.Custom;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeManagement.Data.Entities
{
    // Bảng công thức, định lượng nguyên liệu cho mỗi sản phẩm
    public class Recipe : BaseEntities
    {
        public Guid ProductSizeId { get; set; }

        public Guid IngredientId { get; set; }

        // Lượng nguyên liệu cần dùng (ví dụ: 25 gram)
        [Precision(18, 4)]
        public decimal QuantityUsed { get; set; }

        public ProductSize ProductSize { get; set; } = null!;
        public Ingredient Ingredient { get; set; } = null!;
    }
}