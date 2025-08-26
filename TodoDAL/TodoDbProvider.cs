using Microsoft.EntityFrameworkCore;

namespace TodoDAL
{
    public class TodoDbProvider
    {
        public async Task<TodoItem> AddTodo(TodoItem item)
        {
            using TodoDbContext context = new();
            await context.TodoItems.AddAsync(item);
            await context.SaveChangesAsync();

            return item;
        }

        public async Task<TodoItem> AddTodo(string name, string description, byte priority)
        {
            TodoItem item = new()
            {
                Name = name,
                Description = description,
                Priority = priority,
                CreatedAt = DateTime.UtcNow,
                IsDone = false
            };

            return await AddTodo(item);
        }

        public async Task<IEnumerable<TodoItem>> GetTodos()
        {
            using TodoDbContext context = new();

            return await context.TodoItems.Where(t => !t.IsDone).ToListAsync();
        }

        public async Task<bool> MarkAsDone(int id)
        {
            using TodoDbContext context = new();
            TodoItem? item = await context.TodoItems.SingleOrDefaultAsync(t => t.Id == id);

            if (item != null)
            {
                item.IsDone = true;
                await context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }
    }
}