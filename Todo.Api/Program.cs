

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Data;
using Todo.Api.EndpointDefinitions;

var builder = WebApplication.CreateBuilder(args);

using var cnn = new SqliteConnection("Filename=:memory:");
cnn.Open();
builder.Services.AddDbContext<TodoContext>(options => options.UseSqlite(cnn));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();





var app = builder.Build();
var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<TodoContext>();
DbInitializer.Initialize(db!);

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Hello World!");

app.MapGroup("api/v1")
    .MapTodosApi();



app.Run();
