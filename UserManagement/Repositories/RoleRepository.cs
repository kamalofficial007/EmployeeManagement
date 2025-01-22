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
        private readonly IDbConnection _dbConnection;

        public RoleRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            const string sql = "SELECT * FROM Roles";
            return await _dbConnection.QueryAsync<Role>(sql);
        }

        public async Task AddRoleAsync(Role role)
        {
            const string sql = "INSERT INTO Roles (Name, Description) VALUES (@Name, @Description)";
            await _dbConnection.ExecuteAsync(sql, role);
        }
    }
}
