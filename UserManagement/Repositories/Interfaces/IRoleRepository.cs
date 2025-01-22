using UserManagement.Models;

namespace UserManagement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task AddRoleAsync(Role role);
    }
}
