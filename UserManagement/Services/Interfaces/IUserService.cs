using UserManagement.Models;

namespace UserManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user, string password);
        Task UpdateUserAsync(User user, string password);
        Task DeleteUserAsync(int id);
    }
}
