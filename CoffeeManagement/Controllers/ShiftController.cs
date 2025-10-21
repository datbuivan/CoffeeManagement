using AutoMapper;
using CoffeeManagement.Data.Dtos.Shift;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftsController : BaseController<Shift, ShiftCreateUpdateDto, ShiftCreateUpdateDto, ShiftResultDto>
    {
        public ShiftsController(IGenericRepository<Shift> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }
        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<ShiftResultDto>>> Create([FromBody] ShiftCreateUpdateDto dto)
        {
            return await base.Create(dto);
        }

        // Override Update
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<ShiftResultDto>>> Update(Guid id, [FromBody] ShiftCreateUpdateDto dto)
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