using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class TaskItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public ICollection<SubTaskItem> SubTaskItems { get; set; }
    }
}
