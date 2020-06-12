using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Models.Dtos;
using TodoListApp.Models.DTOs;

namespace TodoListApp.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetCurrentUserAsync();
        Task<User> CreateAccountAsync(CreateUserDto createUserDto);
        Task<string> AuthenticateAsync(AuthenticateRequest request);
    }
}
