using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class SubTask: BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        
        [Required]
        public Task Task { get; set; }
    }
}
