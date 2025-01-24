using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Repositories.Interfaces;

namespace UserManagement.Repositories
{
    public class RoleRepository:IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dbConnection;

        public RoleRepository(AppDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        // Standard CRUD operations using EF
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task AddRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
            }
        }

        // Complex queries using Dapper
        public async Task<IEnumerable<Role>> GetRolesWithUsersCount()
        {
            const string sql = @"
                SELECT r.Id, r.Name, r.Description, COUNT(u.Id) AS UserCount
                FROM Roles r
                LEFT JOIN Users u ON r.Id = u.RoleId
                GROUP BY r.Id, r.Name, r.Description";

            return await _dbConnection.QueryAsync<Role>(sql);
        }
    }
}
