using Dapper;
using System.Data;
using UserManagement.Models;
using UserManagement.Repositories.Interfaces;

namespace UserManagement.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            const string sql = "SELECT u.*, r.Name AS RoleName FROM Users u JOIN Roles r ON u.RoleId = r.Id";
            return await _dbConnection.QueryAsync<User, Role, User>(sql,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                }, splitOn: "RoleId");
        }

        // ... other code ...

        public async Task<User> GetUserByIdAsync(int id)
        {
            const string sql = "SELECT u.*, r.Name AS RoleName FROM Users u JOIN Roles r ON u.RoleId = r.Id WHERE u.Id = @Id";
            var result = await _dbConnection.QueryAsync<User, Role, User>(sql,
                (user, role) =>
                {
                    user.Role = role;
                    return user;
                }, new { Id = id }, splitOn: "RoleId");
            return result.FirstOrDefault();
        }

        public async Task AddUserAsync(User user)
        {
            const string sql = "INSERT INTO Users (Username, Email, PasswordHash, RoleId) VALUES (@Username, @Email, @PasswordHash, @RoleId)";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task UpdateUserAsync(User user)
        {
            const string sql = "UPDATE Users SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash, RoleId = @RoleId WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, user);
        }

        public async Task DeleteUserAsync(int id)
        {
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            await _dbConnection.ExecuteAsync(sql, new { Id = id });
        }
    }
}
