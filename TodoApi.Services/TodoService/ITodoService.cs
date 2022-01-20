using System.Threading.Tasks;
using TodoApi.Domain.DTO;

namespace TodoApi.Services.TodoService
{
    public interface ITodoService
    {
        Task<TodoItemDTO[]> GetTodoItems();
        Task<TodoItemDTO> GetTodoItem(long id);
        Task<bool> UpdateTodoItem(long id, TodoItemDTO todoItemDTO);
        Task<TodoItemDTO> CreateTodoItem(TodoItemDTO item);
        Task<bool> DeleteTodoItem(long id);
    }
}
