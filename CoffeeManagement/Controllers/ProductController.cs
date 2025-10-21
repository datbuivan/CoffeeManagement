using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Product;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using CoffeeManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController
        : BaseController<Product, ProductCreateDto, ProductUpdateDto, ProductResultDto>
    {
        private readonly IProductService _service;
        public ProductController(IGenericRepository<Product> repo, IMapper mapper, IProductService service)
            : base(repo, mapper)
        {
            _service = service;
        }


        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPost("create-with-file")]
        public async Task<ActionResult<ApiResponse<ProductResultDto>>> CreateWithFile([FromForm] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<ProductResultDto>(400, "Invalid product data"));

            try
            {
                var created = await _service.Add(dto);
                return Ok(new ApiResponse<ProductResultDto>(201, "Product created successfully", created));

            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        [Authorize(Roles = "ADMIN,MANAGER")]
        [HttpPut("{id}/with-image")]
        public async Task<ActionResult<ApiResponse<ProductResultDto>>> UpdateWithFile(Guid id, [FromForm] ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Invalid product data"));

            try
            {
                var updated = await _service.Update(id, dto);
                return Ok(new ApiResponse<ProductResultDto>(200, "Product updated successfully", updated));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
