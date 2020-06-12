using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models.Dtos;

namespace TodoListApp.Services
{
    public interface ITaskService
    {
        Task<Models.Task> InsertAsync(TaskDto taskDto);
        Task<Models.Task> GetByUserId(Guid userId);

    }
}
