using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;
using Xunit;

namespace ToDo.Tests
{
    public class UpdateTodo_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoModel result;
        public DateTime expDate = DateTime.Now.AddDays(2);
        private TodoDbContext context;

        public UpdateTodo_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateTodoDatabase")
                .Options;

            context = new TodoDbContext(options);

            context.ToDos.Add(new TodoModel
            {
                Complete = false,
                Completion = 90,
                Description = "test",
                Title = "test",
                CreationDate = new DateTime(),
                Expires = new DateTime()
            });

            context.SaveChanges();

            _todoTaskService = new TodoTaskService(context);

            _todoTaskService.UpdateTodo("test", new TodoModel { Description = "test3", Title = "test3", Expires = expDate });

            result = context.ToDos.First();
        }

        [Fact]
        public void UpdateTodo_should_update_existing_entity()
        {
            result.ShouldBe(new TodoModel { Id = 1, Completion = 90, Complete = false, Description = "test3", Title = "test3", CreationDate = new DateTime(), Expires = expDate });
        }
    }
}
