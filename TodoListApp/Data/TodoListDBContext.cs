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
    public class TodoListDBContext : IdentityDbContext<User, Role, Guid>
    {
        public TodoListDBContext(DbContextOptions<TodoListDBContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var adminRoleGuid = Guid.NewGuid();
            var employeeRoleGuid = Guid.NewGuid();
            var adminUserGuid = Guid.NewGuid();
            builder.Entity<Role>().HasData(
                new Role
                {
                    Id = adminRoleGuid,
                    Name = "Admin",
                    Description = "Admin role",
                    NormalizedName = "ADMIN"
                },
                new Role
                {
                    Id = employeeRoleGuid,
                    Name = "Employee",
                    Description = "Employee role",
                    NormalizedName = "EMPLOYEE"
                }
            );
            var hasher = new PasswordHasher<User>();
            builder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserGuid,
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
            builder.Entity<IdentityUserRole<Guid>> ().HasData(
                new IdentityUserRole<Guid>
                {
                    UserId = adminUserGuid,
                    RoleId = adminRoleGuid,
                }
            );
        }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<SubTask> SubTasks { get; set; }
    }
}
