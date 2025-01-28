using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Services.Interfaces;

namespace UserManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Get all roles.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                return Ok(roles); // 200 OK
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving roles.", ex);
            }
        }

        /// <summary>
        /// Get a role by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRoleById(int id)
        {
            try
            {
                var role = await _roleService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return NotFound(new { Message = $"Role with ID {id} not found." }); // 404 Not Found
                }
                return Ok(role); // 200 OK
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving the role with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole([FromBody] RoleDto roleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // 400 Bad Request
                }

                var createdRole = await _roleService.CreateRoleAsync(roleDto);
                return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole); // 201 Created
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while creating the role.", ex);
            }
        }

        /// <summary>
        /// Update an existing role.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] RoleDto roleDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // 400 Bad Request
                }

                if (id != roleDto.Id)
                {
                    return BadRequest(new { Message = "ID mismatch." }); // 400 Bad Request
                }

                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound(new { Message = $"Role with ID {id} not found." }); // 404 Not Found
                }

                await _roleService.UpdateRoleAsync(roleDto);
                return NoContent(); // 204 No Content
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while updating the role with ID {id}.", ex);
            }
        }

        /// <summary>
        /// Delete a role by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var existingRole = await _roleService.GetRoleByIdAsync(id);
                if (existingRole == null)
                {
                    return NotFound(new { Message = $"Role with ID {id} not found." }); // 404 Not Found
                }

                await _roleService.DeleteRoleAsync(id);
                return NoContent(); // 204 No Content
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the role with ID {id}.", ex);
            }
        }

        [HttpGet("WithUsersCount")]
        public async Task<IActionResult> GetRolesWithUsersCount()
        {
            var roles = await _roleService.GetRolesWithUsersCount();
            return Ok(roles);
        }

    }
}