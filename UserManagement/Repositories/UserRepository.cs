using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UserManagement.Data;
using UserManagement.DTOs;
using UserManagement.Helpers;
using UserManagement.Models;
using UserManagement.Repositories.Interfaces;

namespace UserManagement.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IDbConnection _dbConnection;        


        public UserRepository(AppDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        // CRUD operations using EF
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task<User> AddUserAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                RoleId = userDto.RoleId
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = await _context.Users.FindAsync(userDto.Id);
            if (user != null)
            {
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                user.RoleId = userDto.RoleId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Complex queries using Dapper
        public async Task<IEnumerable<dynamic>> GetUsersWithRolesAsync()
        {
            const string sql = @"
                SELECT u.Id, u.Username, u.Email, u.PasswordHash, r.Name AS RoleName, r.Description AS RoleDescription
                FROM Users u
                INNER JOIN Roles r ON u.RoleId = r.Id";

            return await _dbConnection.QueryAsync(sql);
        }
    }
}
