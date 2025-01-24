using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");


            // Seed data (optional)
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin", Description = "Administrator Role" },
                new Role { Id = 2, Name = "User", Description = "Standard User Role" }
            );
            // Use a precomputed hashed password
            var hashedPassword = "$2a$11$FjO7AqNTWrRpj6JDFh1xB.D2kT8RddoqPCm2F9t0G2.Zb7AWpYtmu"; // Replace with a valid hash

            modelBuilder.Entity<User>().HasData(
               new User
               {
                   Id = 1,
                   Username = "admin",
                   Email = "admin@example.com",
                   PasswordHash = hashedPassword, // Hash the password
                   RoleText = "Admin",
                   RoleId = 1
               }
           );
        }
    }
}
