using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Services.Interfaces;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user, [FromQuery] string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest("Password is required.");

            await _userService.AddUserAsync(user, password);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser, [FromQuery] string password)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound();

            updatedUser.Id = id;
            await _userService.UpdateUserAsync(updatedUser, password);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null) return NotFound();

            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
