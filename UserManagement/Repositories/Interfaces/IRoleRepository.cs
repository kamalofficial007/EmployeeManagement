using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task<Role> AddRoleAsync(RoleDto role);
        Task UpdateRoleAsync(RoleDto role);
        Task DeleteRoleAsync(int id);

        // Complex queries with Dapper
        Task<IEnumerable<Role>> GetRolesWithUsersCount();
    }
}
