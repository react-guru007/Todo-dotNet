using System;

namespace TodoApp.Models
{
    public class Todo
    {
        public int Id { get; set; } // Primary Key

        public string Title { get; set; } // Title of the Todo item

        public string Description { get; set; } // Detailed description of the Todo item

        public bool IsCompleted { get; set; } = false; // Indicates if the Todo item is completed

        public DateTime CreatedAt { get; set; } // Date and time the Todo item was created

        public DateTime? UpdatedAt { get; set; } // Date and time the Todo item was last updated (nullable)

        // Foreign Key referencing the User entity
        public int UserId { get; set; }

        // Navigation property for the related User
        public User User { get; set; }
    }
}
