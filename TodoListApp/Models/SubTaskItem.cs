using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class SubTaskItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Task Task { get; set; }
    }
}
