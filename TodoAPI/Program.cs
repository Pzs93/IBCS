using TodoDAL;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5005);
    options.ListenLocalhost(7005, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

WebApplication app = builder.Build();

app.MapGet("/getTodos", async () =>
{
    TodoDbProvider provider = new();
    IEnumerable<TodoItem> items = await provider.GetTodos();

    return Results.Ok(items);
});

app.MapPost("/addTodo", async (TodoItem item) =>
{
    TodoDbProvider provider = new();
    TodoItem addedItem = await provider.AddTodo(item);

    //return Results.Created($"/getTodo?id={item.Id}", item);
    return Results.Ok(addedItem);
});

app.MapPut("/markAsDone", async (int id) =>
{
    TodoDbProvider provider = new();
    bool result = await provider.MarkAsDone(id);

    return result ? Results.Ok() : Results.NotFound();
});

app.Run();
