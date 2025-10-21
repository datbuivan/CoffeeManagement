using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Table;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    public class TableController
        : BaseController<Table, TableCreateDto, TableUpdateDto, TableResultDto>
    {
        public TableController(IGenericRepository<Table> repo, IMapper mapper)
            : base(repo, mapper)
        {
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<TableResultDto>>> Create([FromBody] TableCreateDto dto)
        {
            return await base.Create(dto);
        }

        // Override Update
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,MANAGER")]
        public override async Task<ActionResult<ApiResponse<TableResultDto>>> Update(Guid id, [FromBody] TableUpdateDto dto)
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

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateTableStatus(Guid id, [FromBody] TableStatusUpdateDto dto)
        {
            var allowedStatuses = new[] { "Available", "Occupied", "Cleaning" };
            if (!allowedStatuses.Contains(dto.Status))
            {
                return BadRequest(new ApiResponse<string>(400, "Invalid status value. Allowed: Available, Occupied, Cleaning."));
            }

            var table = await _repo.GetByIdAsync(id);
            if (table == null)
            {
                return NotFound(new ApiResponse<string>(404, "Table not found"));
            }

            table.Status = dto.Status;
            _repo.Update(table);
            await _repo.SaveChangesAsync();

            return Ok(new ApiResponse<Table>(200, "Status updated successfully", table));
        }
    }
}
