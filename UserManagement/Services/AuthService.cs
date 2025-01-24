using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Helpers;

namespace UserManagement.Services
{
    public class AuthService
    {
        private readonly JwtHelper _jwtHelper;
        private readonly AppDbContext _dbContext;
        public AuthService(AppDbContext context,JwtHelper jwtHelper)
        {
            _jwtHelper = jwtHelper;
            _dbContext = context;
        }


        public string Authenticate(string username, string password)
        {
            // Find the user in the database
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return null; // User not found
            }

            // Verify the password
            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null; // Invalid password
            }

            // Generate and return a JWT
            return _jwtHelper.GenerateToken(user.Username, user.RoleText);
        }
    }
}
