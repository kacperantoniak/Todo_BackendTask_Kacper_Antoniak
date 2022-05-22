using ToDo.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDo.Data
{
    public class TodoDbContext : DbContext
    {
        //represents a table
        public DbSet<TodoModel> ToDos { get; set; }

        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
    }
}
