using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(UserDto user);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int id);

        // Complex queries using Dapper
        Task<IEnumerable<dynamic>> GetUsersWithRolesAsync();
    }
}
