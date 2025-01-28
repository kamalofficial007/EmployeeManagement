using UserManagement.DTOs;
using UserManagement.Models;

namespace UserManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(UserDto user);
        Task UpdateUserAsync(UserDto user);
        Task DeleteUserAsync(int id);

        // Complex query method
        Task<IEnumerable<dynamic>> GetUsersWithRolesAsync();
    }
}
