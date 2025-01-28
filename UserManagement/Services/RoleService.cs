using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Repositories.Interfaces;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services
{
    public class RoleService: IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetRoleByIdAsync(id);
        }

        public async Task<Role> CreateRoleAsync(RoleDto role)
        {
           return await _roleRepository.AddRoleAsync(role);
        }

        public async Task UpdateRoleAsync(RoleDto role)
        {
            await _roleRepository.UpdateRoleAsync(role);
        }

        public async Task DeleteRoleAsync(int id)
        {
            await _roleRepository.DeleteRoleAsync(id);
        }

        public async Task<IEnumerable<Role>> GetRolesWithUsersCount()
        {
            return await _roleRepository.GetRolesWithUsersCount();
        }
    }
}
