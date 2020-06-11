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

namespace TodoListApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private IConfiguration _config;
        private readonly ILogger<WeatherForecastController> _logger;

        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration config, ILogger<WeatherForecastController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create(UserDto userDto)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                FullName = userDto.FullName,
                Email = userDto.Email
            };
            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors);
            }
            var result2 = await _userManager.AddToRoleAsync(user, "Employee");
            if (result2 != IdentityResult.Success)
            {
                return BadRequest(result2.Errors);
            }
            return Ok(user);
        }


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
