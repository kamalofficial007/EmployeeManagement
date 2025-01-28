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

        public async Task<User> AddUserAsync(UserDto user)
        {
            return await _userRepository.AddUserAsync(user);
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            await _userRepository.UpdateUserAsync(userDto);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<IEnumerable<dynamic>> GetUsersWithRolesAsync()
        {
            return await _userRepository.GetUsersWithRolesAsync();
        }
    }
}
