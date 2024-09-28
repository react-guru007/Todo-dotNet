namespace TodoApp.Dtos
{
    public class CreateTodoDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false; // Default value for a new Todo
    }
}