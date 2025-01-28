using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services;
using UserManagement.Services.Interfaces;

namespace UserManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController: ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users); // 200 OK
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving users.", ex);
            }
        }
   

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." }); // 404 Not Found
                }
                return Ok(user); // 200 OK
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the user with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // 400 Bad Request
                }

                await _userService.AddUserAsync(userDto);
                var createdUser = await _userService.GetUserByIdAsync(userDto.Id);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser); // 201 Created
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the user.", ex);
            }
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // 400 Bad Request
                }

                if (id != userDto.Id)
                {
                    return BadRequest(new { Message = "ID mismatch." }); // 400 Bad Request
                }

                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." }); // 404 Not Found
                }

                await _userService.UpdateUserAsync(userDto);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the user with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Delete a user by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound(new { Message = $"User with ID {id} not found." }); // 404 Not Found
                }

                await _userService.DeleteUserAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the user with ID {id}.", ex);
            }

        }

        [HttpGet("WithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            try
            {
                var users = await _userService.GetUsersWithRolesAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving users with roles.", ex);
            }
        }
    }
}
