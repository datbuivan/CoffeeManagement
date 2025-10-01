using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.Ingredient;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Base route: /api/ingredients
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetIngredientById(Guid id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);

            if (ingredient == null)
            {
                return NotFound($"Ingredient with ID {id} not found.");
            }

            return Ok(ingredient);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateIngredient([FromBody] CreateIngredientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newIngredient = await _ingredientService.CreateIngredientAsync(request);

            return CreatedAtAction(nameof(GetIngredientById), new { id = newIngredient.Id }, newIngredient);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateIngredient(Guid id, [FromBody] UpdateIngredientRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _ingredientService.UpdateIngredientAsync(id, request);

            if (!success)
            {
                return NotFound($"Ingredient with ID {id} not found or is not active.");
            }

            return NoContent(); // Trả về 204 No Content khi cập nhật thành công
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteIngredient(Guid id)
        {
            var success = await _ingredientService.DeleteIngredientAsync(id);

            if (!success)
            {
                return NotFound($"Ingredient with ID {id} not found.");
            }

            return NoContent(); // Trả về 204 No Content khi xóa thành công
        }

        [HttpGet("low-stock")] // Route: /api/ingredients/low-stock
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLowStockIngredients()
        {
            var ingredients = await _ingredientService.GetLowStockIngredientsAsync();
            return Ok(ingredients);
        }
    }
}
