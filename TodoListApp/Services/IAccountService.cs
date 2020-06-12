using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;
using TodoListApp.Models.DTOs;

namespace TodoListApp.Services
{
    public interface IAccountService
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetCurrentUser();
        Task<User> CreateAccountAsync(CreateUserDto createUserDto);
    }
}
