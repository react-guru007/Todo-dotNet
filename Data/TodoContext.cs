using Microsoft.EntityFrameworkCore;

namespace TodoApp.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map the User entity to the "users" table in PostgreSQL
            modelBuilder.Entity<User>().ToTable("users"); // Match the exact table name in the database


            // Map the Todo entity to the "todos" table
            modelBuilder.Entity<Todo>().ToTable("todos");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; } // Foreign key reference to the User
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
