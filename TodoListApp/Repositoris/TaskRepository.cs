using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListApp.Repositoris
{
    public class TaskRepository : Repository<Models.Task>, ITaskRepository
    {
        public TaskRepository(TodoListDBContext context) : base(context) { }

        public async Task<IEnumerable<Models.Task>> GetTasksByUserAsync(User user)
        {
            return await entities.Where(task => task.User == user).ToListAsync();
        }
    }
}
