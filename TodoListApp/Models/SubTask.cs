using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class SubTask: BaseEntity
    {
        public string Name { get; set; }
        public Task Task { get; set; }
    }
}
