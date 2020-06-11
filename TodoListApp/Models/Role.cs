using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class Role : IdentityRole<int>
    {
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
    }
}
