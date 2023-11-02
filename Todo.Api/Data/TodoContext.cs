using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api.Data;



public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {

    }

    public DbSet<TodoItem> TodoItems { get; set; }


}