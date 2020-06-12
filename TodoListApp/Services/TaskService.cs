using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Repositoris;

namespace TodoListApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserService _userService;

        public TaskService(ITaskRepository taskRepository, IUserService userService)
        {
            _taskRepository = taskRepository;
            _userService = userService;
        }
        public async Task<Models.Task> CreateTaskAsync(TaskDto taskDto)
        {
            var user = await _userService.GetUserByIdAsync(taskDto.UserId);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var task = new Models.Task
            {
                Name = taskDto.Name,
                DueDate = taskDto.DueDate,
                Priority = taskDto.Priority,
                Description = taskDto.Description,
                User = user,
            };
            await _taskRepository.Insert(task);
            return task;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAll();
        }
        public async Task<IEnumerable<Models.Task>> GetCurrentUserTasksAsync()
        {
            var user = await _userService.GetCurrentUserAsync();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await _taskRepository.GetTasksByUserAsync(user);
        }

    }
}
