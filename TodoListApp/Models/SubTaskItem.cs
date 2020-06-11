using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class SubTaskItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public TaskItem ParentTask { get; set; }
    }
}
