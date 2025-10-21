using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Category;
using CoffeeManagement.Data.Dtos.Recipe;
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
    public class RecipeController
        : BaseController<Recipe, RecipeCreateDto, RecipeUpdateDto, RecipeResultDto>
    {
        public RecipeController(IGenericRepository<Recipe> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<RecipeResultDto>>> Create([FromBody] RecipeCreateDto dto)
        {
            return await base.Create(dto);
        }

        // Override Update
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<RecipeResultDto>>> Update(Guid id, [FromBody] RecipeUpdateDto dto)
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
