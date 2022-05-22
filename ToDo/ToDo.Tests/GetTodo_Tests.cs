using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;
using Xunit;

namespace ToDo.Tests
{
    public class GetTodo_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        private TodoDbContext context;

        public GetTodo_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetTodoDatabase")
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
        public async Task GetTodo_should_return_todomodel()
        {
            (await _todoTaskService.GetTodo("test")).ShouldBe(new TodoModel { Id = 1, Complete = false, Completion = 90, Description = "test", Title = "test", CreationDate = new DateTime(), Expires = new DateTime() });
        }
        
        [Fact]
        public async Task GetTodo_should_return_no_content_204()
        {
            (await _todoTaskService.GetTodo("non-existent_title")).ShouldBe(null);
        }
    }
}
