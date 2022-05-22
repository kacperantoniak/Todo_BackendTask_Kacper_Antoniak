using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Services
{
    public interface ITodoTaskService
    {
        Task<ICollection<TodoModel>> GetAllTodos();
        Task<TodoModel> GetTodo(string title);
        Task<ICollection<TodoModel>> GetIncoming();
        Task CreateTodo(TodoModel todoModel);
        Task<TaskStatus> UpdateTodo(string title, TodoModel request);
        Task<TaskStatus> SetTodoPercentComplete(string title, int percentage);
        Task DeleteTodo(string title);
        Task<TaskStatus> MarkTodoAsDone(string title);
    }
}
