using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Category;
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
    public class CategoryController
        : BaseController<Category, CategoryCreateDto, CategoryUpdateDto, CategoryResultDto>
    {
        public CategoryController(IGenericRepository<Category> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<CategoryResultDto>>> Create([FromBody] CategoryCreateDto dto)
        {
            return await base.Create(dto);
        }

        // Chỉ Admin mới được sửa
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<CategoryResultDto>>> Update(Guid id, [FromBody] CategoryUpdateDto dto)
        {
            return await base.Update(id, dto);
        }

        // Chỉ Admin mới được xóa
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<string>>> Delete(Guid id)
        {
            return await base.Delete(id);
        }
    }
}
