using Microsoft.AspNetCore.Mvc;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Authorization;
using CoffeeManagement.Models.Role;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "ADMIN")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById([FromQuery] string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "Không tìm thấy quyền." });
            }
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest model)
        {
            var result = await _roleService.CreateRoleAsync(model.RoleName, model.Description);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Thêm quyền thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleRequest model)
        {
            var result = await _roleService.UpdateRoleAsync(id, model.NewRoleName, model.NewDescription!);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Cập nhật quyền thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Xóa quyền thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        [HttpPut("assign/{userId}")]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] AssignRolesRequest model)
        {
            var result = await _roleService.UpdateUserRolesAsync(userId, model.RoleNames ?? new List<string>());

            if (result.Succeeded)
            {
                return Ok(new { Message = $"Cập nhật quyền cho nhân viên {userId} thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}
