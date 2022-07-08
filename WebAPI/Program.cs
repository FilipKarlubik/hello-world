using WebAPI.Data;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddSingleton<ITodoServices, TodoService>();
builder.Services.AddSingleton<IUserServices, UserService>();
builder.Services.AddSingleton<IWebServices, WebService>();
builder.Services.AddSingleton<TodoTasksContext>();
var app = builder.Build();
app.UseRouting();
app.MapControllers();
app.Run();