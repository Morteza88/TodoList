using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Models;
using TodoListApp.Models.DTOs;

namespace TodoListApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<User> GetCurrentUser()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            string userName = null;
            foreach (var claim in claims)
            {
                if (claim.Type == ClaimTypes.Name)
                {
                    userName = claim.Value;
                }
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return user;
        }
        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return (user);
        }
        public async Task<User> CreateAccountAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                UserName = createUserDto.UserName,
                FullName = createUserDto.FullName,
                Email = createUserDto.Email
            };
            var CreateUserResult = await _userManager.CreateAsync(user, createUserDto.Password);
            if (CreateUserResult != IdentityResult.Success)
            {
                throw new TaskCanceledException(CreateUserResult.Errors.ToString());
            }
            var addToRoleResult = await _userManager.AddToRoleAsync(user, "Employee");
            if (addToRoleResult != IdentityResult.Success)
            {
                throw new TaskCanceledException(addToRoleResult.Errors.ToString());
            }
            return user;
        }

    }
}
