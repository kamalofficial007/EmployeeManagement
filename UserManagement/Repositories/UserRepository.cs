using Dapper;
using System.Data;
using UserManagement.Models;

namespace UserManagement.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetUsersWithRolesAsync()
        {
            string sql = "SELECT u.*, r.Name AS RoleName FROM Users u JOIN Roles r ON u.RoleId = r.Id";
            return await _dbConnection.QueryAsync<User>(sql);
        }
    }
}
