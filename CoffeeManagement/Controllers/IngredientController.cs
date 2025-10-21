using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.Ingredient;
using CoffeeManagement.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var ingredients = await _ingredientService.GetAllIngredientsAsync();
            return Ok(new ApiResponse<IEnumerable<IngredientResultDto>>(200, "Lấy danh sách nguyên liệu thành công.", ingredients));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var ingredient = await _ingredientService.GetIngredientByIdAsync(id);

            if (ingredient == null)
                return NotFound(new ApiResponse<object>(404, $"Không tìm thấy nguyên liệu với ID {id}."));

            return Ok(new ApiResponse<IngredientResultDto>(200, "Lấy thông tin nguyên liệu thành công.", ingredient));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(400, "Dữ liệu gửi lên không hợp lệ.", ModelState));

            var newIngredient = await _ingredientService.CreateIngredientAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = newIngredient.Id },
                new ApiResponse<IngredientResultDto>(201, "Thêm nguyên liệu mới thành công.", newIngredient));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateIngredientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object>(400, "Dữ liệu cập nhật không hợp lệ.", ModelState));

            var success = await _ingredientService.UpdateIngredientAsync(id, request);

            if (!success)
                return NotFound(new ApiResponse<object>(404, $"Không tìm thấy hoặc nguyên liệu có ID {id} đã ngừng hoạt động."));

            return Ok(new ApiResponse<object>(200, "Cập nhật nguyên liệu thành công."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _ingredientService.DeleteIngredientAsync(id);

            if (!success)
                return NotFound(new ApiResponse<object>(404, $"Không tìm thấy nguyên liệu có ID {id}."));

            return Ok(new ApiResponse<object>(200, "Xóa nguyên liệu thành công."));
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockIngredients()
        {
            var ingredients = await _ingredientService.GetLowStockIngredientsAsync();
            return Ok(new ApiResponse<IEnumerable<IngredientResultDto>>(200, "Lấy danh sách nguyên liệu sắp hết hàng thành công.", ingredients));
        }
    }
}
