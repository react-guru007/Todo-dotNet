using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.Data;
using TodoApp.Dtos;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace TodoApp.Tests
{
    public class TodoControllerTests
    {
        private readonly TodoController _controller;
        private readonly TodoContext _context;
        private readonly Mock<DbSet<Todo>> _mockSet;

        public TodoControllerTests()
        {
            // Setup an in-memory database context
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new TodoContext(options);
            _mockSet = new Mock<DbSet<Todo>>();

            // Initialize the TodoController with the context
            _controller = new TodoController(_context);
        }

        [Fact]
        public async Task CreateTodo_ShouldReturn_CreatedAtAction()
        {
            // Arrange
            var createTodoDto = new CreateTodoDto
            {
                Title = "Test Todo",
                Description = "Test Description",
                IsCompleted = false
            };

            var userId = "1";
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
            };

            // Act
            var result = await _controller.CreateTodo(createTodoDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var todo = Assert.IsType<Todo>(createdResult.Value);
            Assert.Equal(createTodoDto.Title, todo.Title);
            Assert.Equal(createTodoDto.Description, todo.Description);
            Assert.Equal(int.Parse(userId), todo.UserId);
        }

        [Fact]
        public async Task GetTodoById_ShouldReturn_TodoItem()
        {
            // Arrange
            var userId = "1";
            var todo = new Todo
            {
                Id = 1,
                Title = "Test Todo",
                Description = "Test Description",
                IsCompleted = false,
                UserId = int.Parse(userId)
            };
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims)) }
            };

            // Act
            var result = await _controller.GetTodoById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTodo = Assert.IsType<Todo>(okResult.Value);
            Assert.Equal(todo.Id, returnedTodo.Id);
            Assert.Equal(todo.Title, returnedTodo.Title);
            Assert.Equal(todo.Description, returnedTodo.Description);
            Assert.Equal(todo.UserId, returnedTodo.UserId);
        }
    }
}
