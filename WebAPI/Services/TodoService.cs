using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class TodoService : ITodoServices
    {

        private readonly TodoTasksContext _db;

        public TodoService(TodoTasksContext db)
        {
            _db = db;
        }

        public TodoTaskToJSON AddTodoTask(TodoFromInputJSON task)
        {
            if (_db.Users.Any(u => u.Id.Equals(task.UserId)) == false)
                return null;
            
            TodoTask todo = new()
            {
                UserID = task.UserId,
                Title = task.Title
            };
            _db.TodoTasks.Add(todo);
            _db.SaveChanges();
            var todoToJSON = new TodoTaskToJSON(task.UserId, task.Title, todo.Date, todo.IsDone)
            {
                UserName = _db.Users.FirstOrDefault(u => u.Id.Equals(task.UserId)).Name
            };
            return todoToJSON;
        }

        public string DeleteById(int id)
        {
            if (_db.TodoTasks.Any(t => t.Id.Equals(id)))
            {
                TodoTask task = _db.TodoTasks.FirstOrDefault(t => t.Id == id);
                _db.TodoTasks.Remove(task);
                _db.SaveChanges();
                return "[" + task.Title + "] has been deleted";
            }
            else return "Task with id [" + id + "] is not in database";
        }

        public TodoTaskToJSON FinishTodoById(int id)
        {
            if (_db.TodoTasks.Any(t => t.Id.Equals(id)))
            {
                TodoTask todo = _db.TodoTasks.FirstOrDefault(t => t.Id == id);
                todo.IsDone = true;
                _db.TodoTasks.Update(todo);
                _db.SaveChanges();
                var todoTaskToJSON = new TodoTaskToJSON(todo.Id, todo.Title, todo.Date, todo.IsDone);
                todoTaskToJSON.UserName = _db.Users.FirstOrDefault(u => u.Id.Equals(todo.UserID)).Name;

                return todoTaskToJSON;
            }
            else return null;
        }

        public List<TodoTaskToJSON> GetAllTodos()
        {
            var list = new List<TodoTaskToJSON>();
            var todoTasks = _db.TodoTasks.ToList();
            foreach (var todo in todoTasks)
            {
                var todoTaskToJSON = new TodoTaskToJSON(todo.Id, todo.Title, todo.Date, todo.IsDone);
                todoTaskToJSON.UserName = _db.Users.FirstOrDefault(u => u.Id.Equals(todo.UserID)).Name;
                list.Add(todoTaskToJSON);
            }
            
            return list;
        }
    }
}
