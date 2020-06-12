using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            var taskDtos = MapTasksToTaskDtos(tasks);
            return Ok(taskDtos);
        }

        // GET: api/Tasks/GetAllTasksWithDetails
        [HttpGet("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasksWithDetails()
        {
            var tasks = await _taskService.GetAllTasksWithDetailsAsync();
            var taskDtos = MapTasksToTaskDtos(tasks);
            return Ok(taskDtos);
        }

        // POST: api/Tasks/CreateTask
        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Models.Task>> CreateTask(CreateTaskDto createTaskDto)
        {
            var task =  await _taskService.CreateTaskAsync(createTaskDto);
            if (task == null)
            {
                return BadRequest();
            }
            return Ok(task);
        }

        // GET: api/Tasks/GetMyTasks
        [HttpGet("[action]")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            var tasks = await _taskService.GetCurrentUserTasksAsync();
            if (tasks == null)
            {
                return BadRequest();
            }
            var taskDtos = MapTasksToTaskDtos(tasks);
            return Ok(taskDtos);
        }
        private IEnumerable<TaskDto> MapTasksToTaskDtos(IEnumerable<Models.Task> tasks)
        {
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                var taskDto = new TaskDto
                {
                    TaskId = task.Id,
                    Name = task.Name,
                    DueDate = task.DueDate,
                    Priority = task.Priority,
                    Description = task.Description,
                    SubTasks = new List<SubTaskDto>(),
                };
                if (task.User!=null)
                {
                    taskDto.UserId = task.User.Id;
                }
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
            return taskDtos;
        }

        // POST: api/Tasks/AddSubTaskToTask
        [HttpPost("[action]")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<SubTaskDto>> AddSubTaskToTask(SubTaskDto subTaskDto)
        {
            var subTask = await _taskService.AddSubTaskToTaskAsync(subTaskDto);
            if (subTask == null)
            {
                return BadRequest();
            }
            var subTaskDtoOut = new SubTaskDto
            {
                Name = subTask.Name,
                Description = subTask.Description,
                TaskId = subTask.Task.Id,
            };
            return Ok(subTaskDtoOut);
        }
    }
}
