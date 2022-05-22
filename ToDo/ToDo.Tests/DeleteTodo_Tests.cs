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
    public class DeleteTodo_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoModel result;
        public TodoDbContext context;

        public DeleteTodo_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteTodoDatabase")
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

            context.ToDos.Add(new TodoModel
            {
                Complete = true,
                Completion = 30,
                Description = "test2",
                Title = "test2",
                CreationDate = new DateTime(),
                Expires = new DateTime()
            });

            context.SaveChanges();

            _todoTaskService = new TodoTaskService(context);
        }

        [Fact]
        public void DeleteTodo_should_delete_todo()
        {
            _todoTaskService.DeleteTodo("test");
            context.ToDos.First().ShouldBe(new TodoModel { Id = 2, Complete = true, Completion = 30, Description = "test2", Title = "test2", CreationDate = new DateTime(), Expires = new DateTime() });
        }

        [Fact]
        public void DeleteTodo_should_do_nothing_if_todo_is_non_existent()
        {
            _todoTaskService.DeleteTodo("non-existent-todo");
            context.ToDos.First().ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 90, Description = "test", Title = "test", CreationDate = new DateTime(), Expires = new DateTime() });
        }
    }
}
