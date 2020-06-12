using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Repositoris
{
    public interface ISubTaskRepository : IRepository<SubTask>
    {
        Task<List<SubTask>> GetSubTasksByTaskAsync(Models.Task task);
    }
}
