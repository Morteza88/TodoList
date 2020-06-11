using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Services;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtService _jwtService;

        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, IJwtService jwtService, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync<User>();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userManager.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> GetToken([FromForm]TokenRequestDto tokenRequest)
        {
            var user = await _userManager.FindByNameAsync(tokenRequest.Username);
            if (user == null)
                return  BadRequest("Invalid Username or Password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, tokenRequest.Password);
            if (!isPasswordValid)
                return BadRequest("Invalid Username or Password");

            var jwt = await _jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            //_context.Users.Add(user);
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            //var remFromRole = await _userManager.RemoveFromRoleAsync(id, role);

            var results = await _userManager.DeleteAsync(user);

            if (results.Succeeded)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        private bool UserExists(int id)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);
            return user != null;
        }
    }
}
