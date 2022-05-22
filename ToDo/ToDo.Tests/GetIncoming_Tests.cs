using Microsoft.EntityFrameworkCore;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;
using ToDo.Services;
using Xunit;

namespace ToDo.Tests
{
    public class GetIncoming_Tests
    {
        private readonly ITodoTaskService _todoTaskService;
        public DateTime expDate = DateTime.Now.AddHours(2);
        private TodoDbContext context;

        public GetIncoming_Tests()
        {
            var options = new DbContextOptionsBuilder<TodoDbContext>()
              .UseInMemoryDatabase(databaseName: "GetIncomingDatabase")
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
                Expires = expDate
            });

            context.SaveChanges();

            _todoTaskService = new TodoTaskService(context);
        }

        [Fact]
        public async Task GetIncoming_should_return_valid_type()
        {
            (await _todoTaskService.GetIncoming()).First().ShouldBe( new TodoModel { Id = 2, Complete = true, Completion = 30, Description = "test2", Title = "test2", Expires = expDate });
        }

    }
}
