using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models.Dtos
{
    public class SubTaskDto
    {
        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
