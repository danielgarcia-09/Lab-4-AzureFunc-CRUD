using CRUD.Context;
using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Services
{
    public interface ITodoService
    {
        Task<List<Todo>> Get();
        
        Task<Todo> Create(Todo todo);

        Task<Todo> GetById(int id);

        Task<Todo> Update(int id, Todo todo);

        Task<bool> Delete(int id);
    }

    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService( TodoContext context )
        {
            this._context = context;
        } 

        public async Task<List<Todo>> Get()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetById (int id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if (todo is null) return null;

            return todo;
        }

        public async Task<Todo> Create( Todo todo )
        {
            if (todo is null) return null;

            Todo newTodo = new Todo()
            {
                Task = todo.Task,
                Done = false,
                CreatedAt = DateTime.Now.ToShortDateString()
            };
            
            var created = _context.Todos.Add(newTodo);

            await _context.SaveChangesAsync();

            return created.Entity;
        }

        public async Task<Todo> Update(int id, Todo todo)
        {
            var updateTodo = await GetById(id);

            if (updateTodo is null) return null;

            updateTodo.Task = todo.Task;
            updateTodo.Done = todo.Done;

            _context.Update(updateTodo);

            await _context.SaveChangesAsync();

            return updateTodo;
        }

        public async Task<bool> Delete( int id )
        {
            var todo = await GetById(id);

            if (todo is null) return false;

            _context.Todos.Remove(todo);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
