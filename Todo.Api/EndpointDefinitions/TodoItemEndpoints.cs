using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Todo.Api.Data;
using Todo.Api.Models;

namespace Todo.Api.EndpointDefinitions;

public static class TodoItemEndpoints
{
    public static RouteGroupBuilder MapTodosApi(this RouteGroupBuilder endpoints)
    {
        var todoGroup = endpoints.MapGroup("/todos")
        .WithTags("Todos");
        todoGroup.MapGet("/", GetTodos);
        todoGroup.MapGet("/{id}", GetTodo);
        todoGroup.MapPost("/", CreateTodo);
        todoGroup.MapPut("/{id}", UpdateTodo);
        return todoGroup;
    }

    public static async Task<Ok<List<TodoItem>>> GetTodos(TodoContext context)
    {
        var todoItems = await context.TodoItems.ToListAsync();
        return TypedResults.Ok(todoItems);
    }

    public static async Task<Results<Ok<TodoItem>, NotFound>> GetTodo(TodoContext context, int id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(todoItem);
    }

    public static async Task<Results<Created<int>, BadRequest>> CreateTodo(TodoContext context, TodoItem todoItem)
    {
        if (todoItem is null)
        {
            return TypedResults.BadRequest();
        }
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();
        return TypedResults.Created($"todos/{todoItem.Id}", todoItem.Id);
    }

    public static async Task<Results<Ok, BadRequest, NotFound>> UpdateTodo(TodoContext context, int id, TodoItem todoItem)
    {
        bool TodoItemExists = context.TodoItems.Any(e => e.Id == id);

        if (!TodoItemExists)
        {
            return TypedResults.NotFound();
        }

        if (todoItem is null)
        {
            return TypedResults.BadRequest();
        }

        context.Entry(todoItem).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TodoItemExists)
            {
                return TypedResults.NotFound();
            }
            else
            {
                throw;
            }
        }

        return TypedResults.Ok();
    }

    public static async Task<Results<Ok, NotFound>> DeleteTodo(TodoContext context, int id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem is null)
        {
            return TypedResults.NotFound();
        }

        context.TodoItems.Remove(todoItem);
        context.SaveChanges();

        return TypedResults.Ok();
    }

}