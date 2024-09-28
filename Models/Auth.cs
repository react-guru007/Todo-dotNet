using System;
using System.Collections.Generic;

namespace TodoApp.Models
{
    public class User
    {
        public int Id { get; set; } // Primary Key

        public string Username { get; set; } // Unique Username

        public string Email { get; set; } // Unique Email

        public string PasswordHash { get; set; } // Hashed Password

        public DateTime CreatedAt { get; set; } // Date and time the user was created

        // Navigation property for related Todos
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}
