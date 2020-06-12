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
        private readonly IAccountService _accountService;

        public TaskService(ITaskRepository taskRepository, IAccountService accountService)
        {
            _taskRepository = taskRepository;
            _accountService = accountService;
        }

        public async Task<Models.Task> GetByUserId(Guid userId)
        {
            var user = await _accountService.GetUserById(userId);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return await _taskRepository.GetByUser(user);
        }

        public async Task<Models.Task> InsertAsync(TaskDto taskDto)
        {
            var user = await _accountService.GetUserById(taskDto.UserId);
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
    }
}
