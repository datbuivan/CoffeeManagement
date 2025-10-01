using Microsoft.AspNetCore.Mvc;
using CoffeeManagement.Models.User;
using Microsoft.AspNetCore.Authorization;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;

namespace CoffeeManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    // [Authorize(Roles = "ADMIN,MANAGER")] 
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ApplicationUser>>> GetEmployees()
        {
            var employees = await _userService.GetEmployeesAsync();
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateUserRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateEmployeeAsync(model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Thêm nhân viên thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        /// <summary>
        /// PUT: api/users/{id} - Sửa thông tin nhân viên
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateUserRequest model)
        {
            var result = await _userService.UpdateEmployeeAsync(id, model);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Cập nhật thông tin nhân viên thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        /// <summary>
        /// PUT: api/users/{id}/password - Đổi mật khẩu nhân viên (dành cho Admin)
        /// </summary>
        [HttpPut("{id}/password")]
        public async Task<IActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest model)
        {
            var result = await _userService.ChangePasswordAsync(id, model.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Đổi mật khẩu thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }

        /// <summary>
        /// DELETE: api/users/{id} - Vô hiệu hóa (xóa mềm) nhân viên
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var result = await _userService.DeleteEmployeeAsync(id);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Vô hiệu hóa nhân viên thành công." });
            }

            return BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}
