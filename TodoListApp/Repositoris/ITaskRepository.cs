using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Repositoris
{
    public interface ITaskRepository :IRepository<Models.Task>
    {
        Task<Models.Task> GetByName(string firstName);
    }
}
