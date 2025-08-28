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

        public async Task<IEnumerable<TodoItem>> GetTodos()
        {
            using TodoDbContext context = new();
            return await context.TodoItems.Where(x => !x.IsDone).OrderBy(x => x.Priority).ThenBy(x => x.CreatedAt).ToListAsync();
        }

        public async Task<bool> MarkAsDone(int id)
        {
            using TodoDbContext context = new();
            TodoItem? item = await context.TodoItems.SingleOrDefaultAsync(x => x.Id == id);

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