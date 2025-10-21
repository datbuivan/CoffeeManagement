using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.Ingredient;
using System;
using System.Collections.Generic;
using System.Linq; // Cần thiết cho OrderBy
using System.Threading.Tasks;

namespace CoffeeManagement.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IGenericRepository<Ingredient> _ingredientRepository;
        private readonly IMapper _mapper;

        public IngredientService(IGenericRepository<Ingredient> ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IngredientResultDto>> GetAllIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.FindAllAsync(i => i.IsActive);

            var sortedIngredients = ingredients.OrderBy(i => i.Name);

            return _mapper.Map<IEnumerable<IngredientResultDto>>(sortedIngredients);

        }

        public async Task<IngredientResultDto?> GetIngredientByIdAsync(Guid id)
        {
            try
            {
                var ingredients = await _ingredientRepository.FindSingleAsync(i => i.Id == id && i.IsActive);
                return _mapper.Map<IngredientResultDto>(ingredients);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public async Task<IngredientResultDto> CreateIngredientAsync(CreateIngredientRequest request)
        {
            var ingredient = _mapper.Map<Ingredient>(request);
            ingredient.CurrentStock = 0;
            ingredient.IsActive = true;
            ingredient.CreatedAt = DateTime.UtcNow;

            _ingredientRepository.Add(ingredient);
            await _ingredientRepository.SaveChangesAsync();

            return _mapper.Map<IngredientResultDto>(ingredient);
        }

        public async Task<bool> UpdateIngredientAsync(Guid id, UpdateIngredientRequest request)
        {
            try
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(id);

                if (!ingredient.IsActive) return false;

                ingredient.Name = request.Name;
                ingredient.Unit = request.Unit;
                ingredient.ReorderLevel = request.ReorderLevel;
                ingredient.UpdatedAt = DateTime.UtcNow;

                _ingredientRepository.Update(ingredient);
                await _ingredientRepository.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteIngredientAsync(Guid id)
        {
            try
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(id);

                if (!ingredient.IsActive) return true; // Đã bị xóa/ẩn rồi

                ingredient.IsActive = false;
                ingredient.UpdatedAt = DateTime.UtcNow;

                _ingredientRepository.Update(ingredient); // Cập nhật trạng thái
                await _ingredientRepository.SaveChangesAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<IngredientResultDto>> GetLowStockIngredientsAsync()
        {
            var ingredients = await _ingredientRepository.FindAllAsync(
                i => i.IsActive && i.CurrentStock <= i.ReorderLevel
            );

            return _mapper.Map<IEnumerable<IngredientResultDto>>(ingredients);
        }
    }
}