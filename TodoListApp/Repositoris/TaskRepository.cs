using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Data;

namespace TodoListApp.Repositoris
{
    public class TaskRepository : Repository<Models.Task>, ITaskRepository
    {
        public TaskRepository(TodoListDBContext context) : base(context) { }

        public Task<Models.Task> GetByName(string name)
        {
            return context.Set<Models.Task>().FirstOrDefaultAsync(author => author.Name == name);
        }
    }
}
