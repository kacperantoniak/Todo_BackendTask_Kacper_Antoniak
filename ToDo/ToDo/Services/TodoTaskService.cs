using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Data;
using ToDo.Models;

namespace ToDo.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        //database context constructor
        private readonly TodoDbContext _toDoDbContext;

        public TodoTaskService(TodoDbContext toDoDb)
            => _toDoDbContext = toDoDb;

        public async Task<ICollection<TodoModel>> GetAllTodos() 
            => await _toDoDbContext.ToDos.ToListAsync();

        public async Task CreateTodo(TodoModel toDoModel)
        {
            await _toDoDbContext.ToDos.AddAsync(toDoModel);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<TodoModel> GetTodo(string title)
            => await _toDoDbContext
                .ToDos
                .Where(todo => todo.Title.Equals(title))
                .Select(todo => todo)
                .FirstOrDefaultAsync();

        public async Task<ICollection<TodoModel>> GetIncoming()
        {
            return await _toDoDbContext
                .ToDos
                .Where(todo => todo.Expires > DateTime.Now)
                .Where(todo => todo.Expires < DateTime.Now.AddDays(1))
                .Select(todo => todo)
                .ToArrayAsync();
        }

        public async Task<TaskStatus> UpdateTodo(string title, TodoModel request)
        {
            //getting the database object of todo if exists
            if (!_toDoDbContext.ToDos.Where(todo => todo.Title.Equals(title)).Any())
                return await Task.FromResult(TaskStatus.Faulted);

            TodoModel todo = _toDoDbContext
                .ToDos
                .Where(todo => todo.Title.Equals(title))
                .Select(todo => todo)
                .FirstOrDefault();

            //update the properties (this block of code should be in a TodoModelFactory, but I'm not going to make it)
            todo.Title = request.Title;
            todo.Description = request.Description;
            todo.Expires = request.Expires;

            _toDoDbContext.Entry(todo).State = EntityState.Modified;

            await _toDoDbContext.SaveChangesAsync();
            return await Task.FromResult(TaskStatus.RanToCompletion);
        }

        public async Task<TaskStatus> SetTodoPercentComplete(string title, int percentage)
        {  
            if (!_toDoDbContext.ToDos.Where(todo => todo.Title.Equals(title)).Any())
                return await Task.FromResult(TaskStatus.Faulted);
            
            TodoModel todo = _toDoDbContext
                .ToDos
                .Where(todo => todo.Title.Equals(title))
                .Select(todo => todo)
                .FirstOrDefault();

            //this ensures that the percentage value will never exceed 100 or drop below 0 (and hopefully it's faster than if statement)
            todo.Completion = Math.Max(Math.Min(percentage, 100), 0);
                
            _toDoDbContext.Entry(todo).State = EntityState.Modified;

            await _toDoDbContext.SaveChangesAsync();

            return await Task.FromResult(TaskStatus.RanToCompletion);
        }

        public async Task DeleteTodo(string title)
        {
            //getting the database object of todo
            TodoModel todo = _toDoDbContext
                .ToDos
                .Where(todo => todo.Title.Equals(title))
                .Select(todo => todo)
                .FirstOrDefault();

            _toDoDbContext
                .ToDos
                .Remove(todo);
            await _toDoDbContext.SaveChangesAsync();
        }

        public async Task<TaskStatus> MarkTodoAsDone(string title)
        {
            if (!_toDoDbContext.ToDos.Where(todo => todo.Title.Equals(title)).Any())
                return await Task.FromResult(TaskStatus.Faulted);

            TodoModel todo = _toDoDbContext
                .ToDos
                .Where(todo => todo.Title.Equals(title))
                .Select(todo => todo)
                .FirstOrDefault();

            todo.Complete = true;

            _toDoDbContext.Entry(todo).State = EntityState.Modified;

            await _toDoDbContext.SaveChangesAsync();

            return await Task.FromResult(TaskStatus.RanToCompletion);
        }
    }
}
