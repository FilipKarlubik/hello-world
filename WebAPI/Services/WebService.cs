using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class WebService : IWebServices
    {
        private readonly TodoTasksContext _db;

        public WebService(TodoTasksContext db)
        {
            _db = db;
        }

        public void AddTodo(string title, int userId)
        {
            if (_db.Users.Any(u => u.Id == userId))
            {
                TodoTask task = new TodoTask();
                task.Title = title;
                task.UserID = userId;
                _db.TodoTasks.Add(task);
                _db.SaveChanges();
            }
        }

        public List<TodoTask> ListAllTodos()
        {
            return _db.TodoTasks.ToList();
        }

        public List<User> ListAllUsers()
        {
            return _db.Users.ToList();
        }
    }
}
