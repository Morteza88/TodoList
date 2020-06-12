using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Repositoris;
using TodoListApp.Services;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService; 

        public TasksController( ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/Tasks/GetAllTasks
        [HttpGet("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Models.Task>> GetAllTasks()
        {
            return await _taskService.GetAllTasksAsync();
        }

        // POST: api/Tasks/CreateTask
        [HttpPost("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTask(CreateTaskDto createTaskDto)
        {
            var task =  await _taskService.CreateTaskAsync(createTaskDto);
            if (task == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        // GET: api/Tasks/GetMyTasks
        [HttpGet("[action]")]
        //[Authorize(Roles = "Employee")]
        public async Task<ActionResult<List<TaskDto>>> GetMyTasks()
        {
            var tasks = await _taskService.GetCurrentUserTasksAsync();
            if (tasks == null)
            {
                return BadRequest();
            }
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                var taskDto = new TaskDto
                {
                    Name = task.Name,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Description = task.Description,
                    UserId = task.User.Id,
                    SubTasks = new List<SubTaskDto>(),
                };
                if (task.SubTasks != null)
                {
                    foreach (var subTask in task.SubTasks)
                    {
                        taskDto.SubTasks.Add(new SubTaskDto
                        {
                            Name = subTask.Name,
                            Description = subTask.Description,
                            TaskId = task.Id,
                        });
                    }
                }
                taskDtos.Add(taskDto);
            }
            return Ok(taskDtos);
        }

        // POST: api/Tasks/AddDiscriptionToTask
        [HttpPost("[action]")]
        //[Authorize(Roles = "Employee")]
        public async Task<SubTask> AddSubTaskToTask(SubTaskDto subTaskDto)
        {
            return await _taskService.AddSubTaskToTaskAsync(subTaskDto);
        }
    }
}
