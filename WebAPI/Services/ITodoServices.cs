using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Services
{
    public interface ITodoServices
    {
        TodoTaskToJSON AddTodoTask(TodoFromInputJSON task);
        List<TodoTaskToJSON> GetAllTodos();
        string DeleteById(int id);
        TodoTaskToJSON FinishTodoById(int id);
    }
}
