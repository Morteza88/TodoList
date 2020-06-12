using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Models.DTOs;

namespace TodoListApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IConfiguration _config;

        public UserService(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
        }
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<User> GetCurrentUserAsync()
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
        public async Task<User> GetUserByIdAsync(Guid id)
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

        public async Task<string> AuthenticateAsync(AuthenticateRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                throw new Exception("Invalid Username or Password");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
                throw new Exception("Invalid Username or Password");

            var jwtToken = generateJwtToken(user);
            return jwtToken;
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
