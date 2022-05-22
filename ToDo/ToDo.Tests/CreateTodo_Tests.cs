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
    public class CreateTodo_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoModel result;
        private TodoDbContext context;

        public CreateTodo_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateTodoDatabase")
                .Options;

            context = new TodoDbContext(options);

            _todoTaskService = new TodoTaskService(context);

            //date may be incorrect because of async and sync methods running
            _todoTaskService.CreateTodo(new TodoModel { Complete = false, Completion = 90, Description = "test", Title = "test", CreationDate = new DateTime(), Expires = new DateTime() });

            result = context.ToDos.First();
        }

        [Fact]
        public void CreateTodo_should_add_valid_object_to_database()
        {
            result.ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 90, Description = "test", Title = "test" });
        }
    }
}
