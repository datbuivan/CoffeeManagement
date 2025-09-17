using CoffeeManagement.Data.Entities;
using CoffeeManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers() => Ok(await _service.GetUsersAsync());

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(string id) => Ok(await _service.GetUserByIdAsync(id));

        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] ApplicationUser user, [FromQuery] string password)
            => Ok(await _service.AddUserAsync(user, password));

        [HttpPut("users")]
        public async Task<IActionResult> UpdateUser([FromBody] ApplicationUser user)
            => Ok(await _service.UpdateUserAsync(user));

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            await _service.DeleteUserAsync(id);
            return Ok();
        }

    }
}
