using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using System.Security.Claims;
using TodoApp.Dtos;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require authentication for all actions in this controller
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todo
        [HttpGet]
        [Produces("application/json")] // Optional: Specify response format for Swagger

        public async Task<IActionResult> GetTodos()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();
            Console.WriteLine("Authorization Header: " + authHeader);
            // Get user ID from the JWT token claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Retrieve Todo items for the authenticated user
            var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();

            return Ok(todos);
        }

        // GET: api/todo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
                return NotFound(new { message = "Todo item not found" });

            return Ok(todo);
        }

        // POST: api/todo
        [HttpPost]
        [Consumes("application/json")] // Optional: Specify request format for Swagger

        public async Task<IActionResult> CreateTodo([FromBody] CreateTodoDto todo)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            var newTodo = new Todo
            {
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                UserId = userId, // Set the userId based on the authenticated user's context
                CreatedAt = DateTime.UtcNow
            };

            _context.Todos.Add(newTodo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = newTodo.Id }, todo);
        }

        // PUT: api/todo/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] Todo updatedTodo)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
                return NotFound(new { message = "Todo item not found" });

            // Update fields
            todo.Title = updatedTodo.Title;
            todo.Description = updatedTodo.Description;
            todo.IsCompleted = updatedTodo.IsCompleted;
            todo.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        // DELETE: api/todo/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (todo == null)
                return NotFound(new { message = "Todo item not found" });

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Todo item deleted successfully" });
        }
    }
}
