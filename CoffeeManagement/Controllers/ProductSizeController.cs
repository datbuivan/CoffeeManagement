using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.ProductSize;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSizeController
        : BaseController<ProductSize, ProductSizeCreateDto, ProductSizeUpdateDto, ProductSizeResultDto>
    {
        private readonly IProductSizeService _service;
        public ProductSizeController(IGenericRepository<ProductSize> repo, IMapper mapper, IProductSizeService service)
            : base(repo, mapper)
        {
            _service = service;
        }

        [HttpGet("by-product/{productId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductSizeResultDto>>>> GetByProductId(Guid productId)
        {
            var result = await _service.GetByProductIdAsync(productId);

            if (result == null || !result.Any())
            {
                return Ok(new ApiResponse<IEnumerable<ProductSizeResultDto>>(404, "Không tìm size", result));
            }

            return Ok(new ApiResponse<IEnumerable<ProductSizeResultDto>>(200, "Lấy danh sách size thành công", result));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<ProductSizeResultDto>>> Create([FromBody] ProductSizeCreateDto dto)
        {
            return await base.Create(dto);
        }

        // Override Update
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<ProductSizeResultDto>>> Update(Guid id, [FromBody] ProductSizeUpdateDto dto)
        {
            return await base.Update(id, dto);
        }

        // Override Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            return await base.Delete(id);
        }

    }
}

