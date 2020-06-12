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

        //// GET: api/Tasks
        //[HttpGet]
        //public async Task<IEnumerable<Models.Task>> GetUserTasks(Guid UserId)
        //{
        //    return await _taskRepository.GetAll();
        //}
        //[HttpGet]
        //public async Task<IEnumerable<Models.Task>> GetTaskItems()
        //{
        //    return await _taskRepository.GetAll();
        //}

        [HttpPost]
        public async Task<ActionResult> PostTask(TaskDto taskDto)
        {
            var task =  await _taskService.InsertAsync(taskDto);
            if (task == null)
            {
                return BadRequest();
            }
            return Ok();
        }

        // DELETE: api/Tasks/5
        //[HttpDelete("{id}")]
        //public async Task<int> DeleteTask(Guid id)
        //{
        //    return await _taskRepository.Delete(id);
        //}
    }
}
