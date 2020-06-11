﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListApp.Models
{
    public class TaskItem
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DueDate { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public ICollection<SubTaskItem> SubTasks { get; set; }
        public User User { get; set; }
    }
}
