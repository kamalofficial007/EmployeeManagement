using System.Security.Cryptography;
using System.Text;
using UserManagement.DTOs;
using UserManagement.Models;
using UserManagement.Repositories;
using UserManagement.Repositories.Interfaces;
using UserManagement.Services.Interfaces;

namespace UserManagement.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task AddUserAsync(User user, string password)
        {
            user.PasswordHash = HashPassword(password);
            await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(User user, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                user.PasswordHash = HashPassword(password);
            }
            await _userRepository.UpdateUserAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
