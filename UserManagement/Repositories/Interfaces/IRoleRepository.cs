﻿using UserManagement.Models;

namespace UserManagement.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(int id);

        // Complex queries with Dapper
        Task<IEnumerable<Role>> GetRolesWithUsersCount();
    }
}
