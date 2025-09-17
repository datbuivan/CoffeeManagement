using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles() => Ok(await _service.GetRolesAsync());

        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRole(string id) => Ok(await _service.GetRoleByIdAsync(id));

        [HttpPost("roles")]
        public async Task<IActionResult> AddRole([FromBody] ApplicationRole role)
            => Ok(await _service.AddRoleAsync(role));

        [HttpPut("roles")]
        public async Task<IActionResult> UpdateRole([FromBody] ApplicationRole role)
            => Ok(await _service.UpdateRoleAsync(role));

        [HttpDelete("roles/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await _service.DeleteRoleAsync(id);
            return Ok();
        }
    }
}
