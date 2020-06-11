using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Data
{
    public class TodoListDBContext : IdentityDbContext<User, Role, int>
    {
        public TodoListDBContext(DbContextOptions<TodoListDBContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = 1,
                    Name = "Admin",
                    Description = "Admin role",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = 2,
                    Name = "Employee",
                    Description = "Employee role",
                    NormalizedName = "EMPLOYEE"
                }
            );
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "Admin",
                    PasswordHash = hasher.HashPassword(null, "Admin123!@#"),
                    FullName = "Administrator",
                    NormalizedUserName = "admin",
                    Email = "admin@email.com",
                    NormalizedEmail = "admin@email.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            );
            builder.Entity<IdentityUserRole<int>> ().HasData(
                new IdentityUserRole<int>
                {
                    UserId = 1,
                    RoleId = 1,
                }
            );
        }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<SubTaskItem> SubTaskItems { get; set; }
    }
}
