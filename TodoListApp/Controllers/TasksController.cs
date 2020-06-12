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

        // GET: api/Tasks
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<Models.Task>> GetAllTasks()
        {
            return await _taskService.GetAllTasksAsync();
        }

        // GET: api/Tasks/GetMyTasks
        [HttpGet("[action]")]
        //[Authorize(Roles = "Employee")]
        public async Task<IEnumerable<Models.Task>> GetMyTasks()
        {
            return await _taskService.GetCurrentUserTasksAsync();
        }

        // GET: api/Tasks
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateTask(TaskDto taskDto)
        {
            var task =  await _taskService.CreateTaskAsync(taskDto);
            if (task == null)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
