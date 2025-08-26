using TodoDAL;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/addTodo", async (string name, string description, byte priority) =>
{
    TodoDbProvider provider = new();
    TodoItem item = await provider.AddTodo(name, description, priority);

    return Results.Created($"/getTodo?id={item.Id}", item);
});

app.MapGet("/getTodos", async () =>
{
    TodoDbProvider provider = new();
    IEnumerable<TodoItem> items = await provider.GetTodos();

    return Results.Ok(items);
});

app.MapGet("/markAsDone", async (int id) =>
{
    TodoDbProvider provider = new();
    bool result = await provider.MarkAsDone(id);

    return result ? Results.Ok() : Results.NotFound();
});

app.Run();
