using AutoMapper;
using CoffeeManagement.Data.Dtos;
using CoffeeManagement.Data.Dtos.Table;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
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

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateTableStatus(Guid id, [FromBody] TableStatusUpdateDto dto)
        {
            var allowedStatuses = new[] { "Available", "Occupied", "Cleaning" };
            if (!allowedStatuses.Contains(dto.Status))
            {
                return BadRequest("Invalid status value. Allowed: Available, Occupied, Cleaning.");
            }

            var table = await _repo.GetByIdAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            table.Status = dto.Status;
            _repo.Update(table);
            await _repo.SaveChangesAsync();

            return Ok(new { message = "Status updated successfully", status = table.Status });
        }
    }
}
