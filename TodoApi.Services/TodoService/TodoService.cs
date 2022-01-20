using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.DAL;
using TodoApi.DAL.Entity;
using TodoApi.Domain.DTO;

namespace TodoApi.Services.TodoService
{
    public class TodoService : ITodoService
    {
        private readonly TodoContext _context;
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoItemDTO> CreateTodoItem(TodoItemDTO item)
        {
            var todoItem = new TodoItem
            {
                IsComplete = item.IsComplete,
                Name = item.Name
            };

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return ItemToDTO(todoItem);

           
        }

        public async Task<bool> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return false;
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<TodoItemDTO> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
        }

        public Task<TodoItemDTO[]> GetTodoItems()
        {
            return _context.TodoItems.Select(x => ItemToDTO(x)).ToArrayAsync();
        }

        public async Task<bool> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return false;
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return false;
            }

            return true;
        }

        private bool TodoItemExists(long id) =>
                _context.TodoItems.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
