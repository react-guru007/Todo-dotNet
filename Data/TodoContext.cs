using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

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

}
