using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models.Dtos
{
    public class TaskDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
    }
}
