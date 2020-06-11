using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Models;

namespace TodoListApp.Services
{
    public interface IJwtService
    {
        Task<AccessToken> GenerateAsync(User user);
    }
}
