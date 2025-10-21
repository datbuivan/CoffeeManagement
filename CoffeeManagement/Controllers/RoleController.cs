using Microsoft.AspNetCore.Mvc;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.Role;
using CoffeeManagement.Errors;
using CoffeeManagement.Data.Entities;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Get()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(new ApiResponse<IEnumerable<ApplicationRole>>(200, "Lấy danh sách quyền thành công", roles));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound(new ApiResponse<string>(404, "Không tìm thấy quyền."));
            }

            return Ok(new ApiResponse<ApplicationRole>(200, "Lấy quyền thành công", role));
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest model)
        {
            var result = await _roleService.CreateRoleAsync(model.RoleName, model.Description);

            if (result.Succeeded)
            {
                return Ok(new ApiResponse<string>(200, "Thêm quyền thành công."));
            }

            return BadRequest(new ApiResponse<IEnumerable<string>>(400, "Thêm quyền thất bại.", result.Errors.Select(e => e.Description)));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleRequest model)
        {
            var result = await _roleService.UpdateRoleAsync(id, model.NewRoleName, model.NewDescription!);

            if (result.Succeeded)
            {
                return Ok(new ApiResponse<string>(200, "Cập nhật quyền thành công."));
            }

            return BadRequest(new ApiResponse<IEnumerable<string>>(400, "Cập nhật quyền thất bại.", result.Errors.Select(e => e.Description)));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _roleService.DeleteRoleAsync(id);

            if (result.Succeeded)
            {
                return Ok(new ApiResponse<string>(200, "Xóa quyền thành công."));
            }

            return BadRequest(new ApiResponse<IEnumerable<string>>(400, "Xóa quyền thất bại.", result.Errors.Select(e => e.Description)));
        }

        [HttpPut("assign/{userId}")]
        public async Task<IActionResult> AssignRoles(string userId, [FromBody] AssignRolesRequest model)
        {
            var result = await _roleService.UpdateUserRolesAsync(userId, model.RoleNames ?? new List<string>());

            if (result.Succeeded)
            {
                return Ok(new ApiResponse<string>(200, $"Cập nhật quyền cho nhân viên {userId} thành công."));
            }

            return BadRequest(new ApiResponse<IEnumerable<string>>(400, $"Cập nhật quyền cho nhân viên {userId} thất bại.", result.Errors.Select(e => e.Description)));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRolesByUserId(string userId)
        {
            var roles = await _roleService.GetRolesByUserIdAsync(userId);
            if (!roles.Any())
                return NotFound(new ApiResponse<string>(404, $"Không tìm thấy quyền cho user {userId}."));

            return Ok(new ApiResponse<IEnumerable<string>>(200, "Lấy danh sách quyền của user thành công.", roles));
        }

        [HttpPost("by-user-ids")]
        public async Task<IActionResult> GetRolesByUserIds([FromBody] IEnumerable<string> userIds)
        {
            if (userIds == null || !userIds.Any())
            {
                return BadRequest(new ApiResponse<string>(400, "Danh sách userId không được để trống."));
            }

            var rolesByUsers = await _roleService.GetRolesByUserIdsAsync(userIds);

            return Ok(new ApiResponse<Dictionary<string, IEnumerable<string>>>(
                200,
                "Lấy danh sách quyền của nhiều user thành công.",
                rolesByUsers
            ));
        }

    }
}
