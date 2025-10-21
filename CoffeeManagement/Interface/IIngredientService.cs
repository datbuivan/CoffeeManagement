using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Models.Ingredient;

namespace CoffeeManagement.Interface
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientResultDto>> GetAllIngredientsAsync();
        Task<IngredientResultDto?> GetIngredientByIdAsync(Guid id);

        Task<IngredientResultDto> CreateIngredientAsync(CreateIngredientRequest request);
        Task<bool> UpdateIngredientAsync(Guid id, UpdateIngredientRequest request);
        Task<bool> DeleteIngredientAsync(Guid id);

        // Chức năng đặc biệt: Lấy nguyên liệu sắp hết hàng
        Task<IEnumerable<IngredientResultDto>> GetLowStockIngredientsAsync();
    }
}