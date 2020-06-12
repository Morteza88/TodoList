using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Repositoris
{
    public interface ITaskRepository :IRepository<Models.Task>
    {
        Task<Models.Task> GetByUser(User user);
    }
}
