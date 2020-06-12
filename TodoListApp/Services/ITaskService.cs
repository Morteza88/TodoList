using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models.Dtos;

namespace TodoListApp.Services
{
    public interface ITaskService
    {
        Task<Models.Task> CreateTaskAsync(TaskDto taskDto);
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<IEnumerable<Models.Task>> GetCurrentUserTasksAsync();

    }
}
