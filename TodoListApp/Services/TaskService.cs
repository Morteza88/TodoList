using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly ISubTaskRepository _subTaskRepository;
        private readonly IUserService _userService;

        public TaskService(ITaskRepository taskRepository, ISubTaskRepository subTaskRepository, IUserService userService)
        {
            _taskRepository = taskRepository;
            _subTaskRepository = subTaskRepository;
            _userService = userService;
        }

        public async Task<Models.Task> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var user = await _userService.GetUserByIdAsync(createTaskDto.UserId);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var task = new Models.Task
            {
                Name = createTaskDto.Name,
                DueDate = createTaskDto.DueDate,
                Priority = createTaskDto.Priority,
                Description = createTaskDto.Description,
                SubTasks = new List<SubTask>(),
                User = user,
            };
            await _taskRepository.InsertAsync(task);
            return task;
        }

        public async Task<IEnumerable<Models.Task>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllAsync();
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
        public async Task<SubTask> AddSubTaskToTaskAsync(SubTaskDto subTaskDto)
        {
            var task = await _taskRepository.GetByIdAsync(subTaskDto.TaskId);
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            var subTask = new SubTask
            {
                Name = subTaskDto.Name,
                Description = subTaskDto.Description,
                Task = task,
            };
            await _subTaskRepository.InsertAsync(subTask);
            return subTask;
        }

    }
}
