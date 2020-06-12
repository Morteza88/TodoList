using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;

namespace TodoListApp.Services
{
    public interface ITaskService
    {
        Task<Models.Task> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<IEnumerable<Models.Task>> GetAllTasksAsync();
        Task<IEnumerable<Models.Task>> GetCurrentUserTasksAsync();
        Task<SubTask> AddSubTaskToTaskAsync(SubTaskDto subTaskDto);
    }
}
