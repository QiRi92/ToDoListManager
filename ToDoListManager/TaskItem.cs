using System;

namespace ToDoListManager
{
    // Represents a single task in the to-do list
    public class TaskItem
    {
        
        // Task description (the text you type in)
        public string Description { get; set; }

        // Whether the task is marked as completed
        public bool IsCompleted { get; set; }

        // Override ToString so ListBox shows nice text
        public override string ToString()
        {
            // If completed → show with [✔]
            // If not → show with [ ]
            return (IsCompleted ? "[✔] " : "[ ] ") + Description;
        }
    }
}
