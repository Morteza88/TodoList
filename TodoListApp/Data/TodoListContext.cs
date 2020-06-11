using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Data
{
    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options)
            : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<SubTaskItem> SubTaskItems { get; set; }
    }
}
