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
    public class SetTodoPercentComplete_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoModel result;
        private TodoDbContext context;

        public SetTodoPercentComplete_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "SetTodoPercentCompleteDatabase")
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
        }

        [Fact]
        public void SetTodoPercentComplete_should_set_10()
        {
            _todoTaskService.SetTodoPercentComplete("test", 10);
            context.ToDos.First().ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 10, Description = "test", Title = "test" });
        }

        [Fact]
        public void SetTodoPercentComplete_should_set_0_with_input_negative20()
        {
            _todoTaskService.SetTodoPercentComplete("test", -20);
            context.ToDos.First().ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 0, Description = "test", Title = "test" });
        }

        [Fact]
        public void SetTodoPercentComplete_should_set_100_with_input_200()
        {
            _todoTaskService.SetTodoPercentComplete("test", 200);
            context.ToDos.First().ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 100, Description = "test", Title = "test" });
        }
    }
}
