using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq; // Cần thiết cho OrderBy
using System.Threading.Tasks;

namespace CoffeeManagement.Services
{
    // Đã thay thế ApplicationDbContext bằng IGenericRepository<Ingredient>
    public class IngredientService : IIngredientService
    {
        private readonly IGenericRepository<Ingredient> _ingredientRepository;

        // Inject IGenericRepository<Ingredient>
        public IngredientService(IGenericRepository<Ingredient> ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.ListAllAsync();
            // Xử lý logic sorting và filtering (Active) trên Service
            return ingredients
                .Where(i => i.IsActive)
                .OrderBy(i => i.Name)
                .ToList();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(Guid id)
        {
            try
            {
                // Sử dụng FindSingleAsync thay vì GetByIdAsync để có thể kiểm tra IsActive
                return await _ingredientRepository.FindSingleAsync(i => i.Id == id && i.IsActive);
            }
            catch (InvalidOperationException)
            {
                return null; // Trả về null nếu không tìm thấy
            }
        }

        public async Task<Ingredient> CreateIngredientAsync(CreateIngredientRequest request)
        {
            var ingredient = new Ingredient
            {
                Name = request.Name,
                Unit = request.Unit,
                CurrentStock = request.CurrentStock,
                ReorderLevel = request.ReorderLevel,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _ingredientRepository.Add(ingredient);
            await _ingredientRepository.SaveChangesAsync();

            return ingredient;
        }

        public async Task<bool> UpdateIngredientAsync(Guid id, UpdateIngredientRequest request)
        {
            try
            {
                // Sử dụng GetByIdAsync để lấy entity đang được theo dõi
                var ingredient = await _ingredientRepository.GetByIdAsync(id);

                // Kiểm tra trạng thái Active, nếu không Active thì không cho cập nhật
                if (!ingredient.IsActive) return false;

                ingredient.Name = request.Name; // Giả sử UpdateIngredientRequest có NewName
                ingredient.Unit = request.Unit;
                ingredient.ReorderLevel = request.ReorderLevel;
                ingredient.UpdatedAt = DateTime.UtcNow;

                // Dùng phương thức Update (nếu GetByIdAsync không Attach entity)
                // Tuy nhiên, GetByIdAsync thường trả về tracked entity, nên ta chỉ cần SaveChangesAsync
                // Nếu Repository có logic Attach/Modified (như trong code bạn cung cấp), 
                // thì ta cần gọi Update(ingredient) nếu GetByIdAsync() không theo dõi entity.
                _ingredientRepository.Update(ingredient); // Gọi phương thức Update từ Repository

                await _ingredientRepository.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false; // Entity not found
            }
        }

        public async Task<bool> DeleteIngredientAsync(Guid id)
        {
            try
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(id);

                if (!ingredient.IsActive) return true; // Đã bị xóa/ẩn rồi

                // Soft Delete (chỉ ẩn, không xóa khỏi DB)
                ingredient.IsActive = false;
                ingredient.UpdatedAt = DateTime.UtcNow;

                _ingredientRepository.Update(ingredient); // Cập nhật trạng thái
                await _ingredientRepository.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false; // Entity not found
            }
        }

        public async Task<IEnumerable<Ingredient>> GetLowStockIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.ListAllAsync();
            // Xử lý logic nghiệp vụ về tồn kho thấp
            return ingredients
                 .Where(i => i.IsActive && i.CurrentStock <= i.ReorderLevel)
                 .ToList();
        }
    }
}