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
    public class MarkTodoAsDone_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        private TodoDbContext context;

        public MarkTodoAsDone_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "MarkTodoAsDoneDatabase")
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
        public void MarkTodoAsDone_should_mark_as_done()
        {
            _todoTaskService.MarkTodoAsDone("test");
            context.ToDos.First().ShouldBe(new TodoModel { Id = 1, Completion = 90, Complete = true, Description = "test", Title = "test", CreationDate = new DateTime(), Expires = new DateTime() });
        }

        [Fact]
        public void MarkTodoAsDone_should_return_null_if_todo_is_nonexistent()
        {
            _todoTaskService.MarkTodoAsDone("test2");
            context.ToDos.Find(2).ShouldBe(null);
        }
    }
}
