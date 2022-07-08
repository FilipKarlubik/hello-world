using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IWebServices
    {
        List<TodoTask> ListAllTodos();
        List<User> ListAllUsers();
        void AddTodo(string title, int userId);
    }
}
