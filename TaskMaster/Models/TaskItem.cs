// Models/TaskItem.cs
using System;

namespace TaskMaster.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? DueTime { get; set; }
        public string Priority { get; set; }
        public string Category { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsImportant { get; set; }
    }
}