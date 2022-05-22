using ToDo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ToDo.Services;

namespace ToDo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;

        public TodoTaskController(ITodoTaskService toDoTaskService)
        {
            _todoTaskService = toDoTaskService;
        }

        [HttpGet]
        [Route("GetAllTodos")]
        public async Task<IActionResult> GetAll()
            => Ok(await _todoTaskService.GetAllTodos());

        [HttpGet]
        [Route("GetTodo/{title}")]
        public async Task<IActionResult> Get(string title) 
            => Ok(await _todoTaskService.GetTodo(title));

        [HttpGet]
        [Route("GetIncomingTodos")]
        public async Task<IActionResult> GetIncoming()
        {
            return Ok(await _todoTaskService.GetIncoming());
        }

        [HttpPost]
        [Route("CreateTodo")]
        public async Task<IActionResult> Create([FromBody] TodoModel toDoTask)
        {
            await _todoTaskService.CreateTodo(toDoTask);
            return Ok();
        }

        [HttpPut]
        [Route("UpdateTodo/{title}")]
        public async Task<IActionResult> Update(string title, TodoModel request)
        {
            _ = await _todoTaskService.UpdateTodo(title, request);
            return Ok();
        }

        [HttpPut]
        [Route("SetTodoPercentComplete/{title}/Completed{percentage}%")]
        public async Task<IActionResult> SetPercentComplete(string title, int percentage)
            => Ok(await _todoTaskService.SetTodoPercentComplete(title, percentage));

        [HttpDelete]
        [Route("DeleteTodo/{title}")]
        public IActionResult Delete(string title)
        {
            _ = _todoTaskService.DeleteTodo(title);
            return Ok();
        }

        [HttpPut]
        [Route("MarkTodoAsDone/{title}")]
        public async Task<IActionResult> MarkAsDone(string title)
            => Ok(await _todoTaskService.MarkTodoAsDone(title));

    }
}
