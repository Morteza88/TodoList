using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Models;
using TodoListApp.Models.DTOs;

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<User>> GetMyUser(int id)
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            string userName = null;
            foreach (var claim in claims)
            {
                if (claim.Type==ClaimTypes.Name)
                {
                    userName = claim.Value;
                }
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.UserName = userDto.UserName;
            //user.Password = userDto.Password;
            user.Email = userDto.Email;
            user.FullName = userDto.FullName;
            var result = await _userManager.UpdateAsync(user);

            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDto userDto)
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

            //return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            // Look for user in the UserStore
            var user = _userManager.Users.SingleOrDefault(u => u.Id == id);

            // If not found, exit
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Get user roles!
            var roles = await _userManager.GetRolesAsync(user);
            // Remove user from roles
            foreach (var role in roles)
            {
                var removeFromRole = await _userManager.RemoveFromRoleAsync(user, role);
                if (removeFromRole != IdentityResult.Success)
                {
                    return BadRequest(removeFromRole.Errors);
                }
            }

            // Remove user from UserStore
            var result = await _userManager.DeleteAsync(user);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors);
            }
            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return _userManager.Users.Any(u => u.Id == id);
        }
    }
}
