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
            builder.Entity<Role>().HasData(new List<Role>
            {
                new Role {
                    Id = 1,
                    Name = "Admin",
                    Description = "Admin role",
                    NormalizedName = "ADMIN"
                },
                new Role {
                    Id = 2,
                    Name = "Employee",
                    Description = "Employee role",
                    NormalizedName = "EMPLOYEE"
                },
            });
        }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<SubTaskItem> SubTaskItems { get; set; }
    }
}
