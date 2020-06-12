using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Models.DTOs;
using TodoListApp.Services;

namespace TodoListApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<WeatherForecastController> _logger;

        public UsersController(IUserService userService, ILogger<WeatherForecastController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            if (users == null)
            {
                return BadRequest();
            }
            return Ok(users);
        }

        // POST: api/Users/CreateUser
        [HttpPost("[action]")]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto createUserDto)
        {
            var user = await _userService.CreateAccountAsync(createUserDto);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        // POST: api/Users/Authenticate
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate(AuthenticateRequest request)
        {
            var jwtToken = await _userService.AuthenticateAsync(request);
            if (jwtToken==null)
            {
                return BadRequest();
            }
            return Ok(jwtToken);
        }
    }
}
