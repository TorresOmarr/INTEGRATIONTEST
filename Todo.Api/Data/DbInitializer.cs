using Todo.Api.Models;

namespace Todo.Api.Data;



public static class DbInitializer
{
    public static void Initialize(TodoContext context)
    {
        context.Database.EnsureCreated();

        if (context.TodoItems.Any())
        {
            return;
        }

        var todos = new TodoItem[]
        {
                new TodoItem(){Name = "Learn C#"},
                new TodoItem(){Name = "Learn ASP.NET Core"},
                new TodoItem(){Name = "Build a web app"},
        };

        context.TodoItems.AddRange(todos);

        context.SaveChanges();



    }
}