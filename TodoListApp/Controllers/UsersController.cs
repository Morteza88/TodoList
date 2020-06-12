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
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private IConfiguration _config;
        private readonly ILogger<WeatherForecastController> _logger;

        public UsersController(UserManager<User> userManager, IUserService userService, IConfiguration config, ILogger<WeatherForecastController> logger)
        {
            _userManager = userManager;
            _userService = userService;
            _config = config;
            _logger = logger;
        }

        // GET: api/Users
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userService.GetUsersAsync();
        }

        // POST: api/Users
        [HttpPost]
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
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return BadRequest("Invalid Username or Password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                return BadRequest("Invalid Username or Password");

            var jwtToken = generateJwtToken(user);
            return Ok(jwtToken);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Role, "Admin"),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
