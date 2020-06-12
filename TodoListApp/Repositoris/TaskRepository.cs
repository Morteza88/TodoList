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

        public Task<Models.Task> GetByUser(User user)
        {
            return context.Set<Models.Task>().FirstOrDefaultAsync(tast => tast.User == user);
        }
    }
}
