using AutoMapper;
using CoffeeManagement.Data.Entities;
using CoffeeManagement.Errors;
using CoffeeManagement.Interface;
using CoffeeManagement.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public UserController(IUserService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Get()
        {
            try
            {
                var users = await _service.Get();
                return Ok(new ApiResponse<IReadOnlyList<UserResultDto>>(200, "Lấy danh sách nhân viên thành công", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var user = await _service.GetById(id);
                if (user == null)
                    return NotFound(new ApiResponse<string>(404, "Nhân viên không tồn tại"));

                return Ok(new ApiResponse<UserResultDto>(200, "Lấy thông tin nhân viên thành công", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("non-admin")]
        public async Task<ActionResult> GetNonAdminUsers()
        {
            try
            {
                var users = await _service.GetNonAdminUsers();
                return Ok(new ApiResponse<IReadOnlyList<UserResultDto>>(200, "Lấy danh sách nhân viên (không bao gồm ADMIN) thành công", users));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // POST: api/users
        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]

        public async Task<ActionResult> Create([FromForm] CreateUserRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Dữ liệu không hợp lệ"));

            try
            {
                var user = await _service.Create(model);
                return Ok(new ApiResponse<UserResultDto>(201, "Tạo nhân viên thành công", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // PUT: api/users/{id}
        [HttpPut("{id}/update")]
        [Authorize(Roles = "ADMIN")]

        public async Task<ActionResult> Update(string id, [FromForm] UpdateUserRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string>(400, "Dữ liệu không hợp lệ"));

            try
            {
                var user = await _service.Update(id, model);
                return Ok(new ApiResponse<UserResultDto>(200, "Cập nhật nhân viên thành công", user));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // PUT: api/users/{id}/password
        [HttpPut("{id}/password")]
        public async Task<ActionResult> ChangePassword(string id, [FromBody] ChangePasswordRequest model)
        {
            try
            {
                var result = await _service.ChangePassword(id, model.NewPassword);
                if (!result)
                    return BadRequest(new ApiResponse<bool>(400, "Đổi mật khẩu thất bại hoặc nhân viên không tồn tại", false));

                return Ok(new ApiResponse<bool>(200, "Đổi mật khẩu thành công", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (!result)
                    return NotFound(new ApiResponse<bool>(404, "Nhân viên không tồn tại", false));

                return Ok(new ApiResponse<bool>(200, "Vô hiệu hóa nhân viên thành công", true));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(500, $"Internal Server Error: {ex.Message}"));
            }
        }
    }
}
