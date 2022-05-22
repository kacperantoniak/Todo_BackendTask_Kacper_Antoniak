using System;
using ToDo.Services;
using Xunit;
using ToDo.Models;
using ToDo.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Shouldly;

namespace ToDo.Tests
{
    public class GetAllTodos_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        private TodoDbContext context;

        public GetAllTodos_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetAllTodosDatabase")
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
        public async Task GetAllTodos_should_return_todo_model()
        {
            (await _todoTaskService.GetAllTodos()).First().ShouldBe(new TodoModel {Id = 1, Complete = false, Completion = 90, Description = "test", Title = "test", CreationDate = new DateTime(), Expires = new DateTime() });
        }
    }
}
