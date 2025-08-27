using TodoDAL;

namespace TodoWpfClient.API
{
    public class TodoApiClient : ApiClientBase
    {
        public TodoApiClient(string apiUrl) : base(apiUrl)
        {
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            return await GetAsync<IEnumerable<TodoItem>>("getTodos");
        }

        public async Task<TodoItem> AddTodoItem(TodoItem item)
        {
            return await PostAsync<TodoItem, TodoItem>("addTodo", item);
        }

        public async Task MarkAsDone(int id)
        {
            await PutAsync($"markAsDone?id={id}", new { });
        }
    }
}