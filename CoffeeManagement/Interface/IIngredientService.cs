using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models.Ingredient;

namespace CoffeeManagement.Interface
{
    public interface IIngredientService
    {
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient?> GetIngredientByIdAsync(Guid id);

        Task<Ingredient> CreateIngredientAsync(CreateIngredientRequest request);
        Task<bool> UpdateIngredientAsync(Guid id, UpdateIngredientRequest request);
        Task<bool> DeleteIngredientAsync(Guid id);

        // Chức năng đặc biệt: Lấy nguyên liệu sắp hết hàng
        Task<IEnumerable<Ingredient>> GetLowStockIngredientsAsync();
    }
}