using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> CreateRoleAsync(RoleDto roleDto);
        Task UpdateRoleAsync(RoleDto roleDto);
        Task DeleteRoleAsync(int id);
        Task<IEnumerable<Role>> GetRolesWithUsersCount();
    }
}
