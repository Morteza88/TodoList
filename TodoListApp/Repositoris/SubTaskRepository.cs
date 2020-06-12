using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListApp.Repositoris
{
    public class SubTaskRepository : Repository<SubTask>, ISubTaskRepository
    {
        public SubTaskRepository(TodoListDBContext context) : base(context) { }

        public async Task<List<SubTask>> GetSubTasksByTaskAsync(Models.Task task)
        {
            return await context.Set<SubTask>().Where(subTask => subTask.Task == task).ToListAsync();
        }
    }
}
