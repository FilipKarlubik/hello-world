using System.Text;
using System.Linq;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class UserService : IUserServices
    {
        private readonly TodoTasksContext _db;

        public UserService(TodoTasksContext db)
        {
            _db = db;
        }

        public string AddUser(string name)
        {
            if(_db.Users.ToList().Any(u => u.Name.Equals(name)))
                return "User with name " + name + " already exists";
            
            _db.Users.Add(new User()
            {
                Name = name
            });
            _db.SaveChanges();
            return "User " + name + " added";
        }

        public string GetTodosById(int id)
        {
            if (_db.Users.Any(u => u.Id.Equals(id)))
            {
                StringBuilder sb = new StringBuilder();
                sb = sb.Append(_db.Users.FirstOrDefault(u => u.Id.Equals(id)).Name + "'s todos: ");
                List<TodoTask> list = _db.TodoTasks.ToList();           
                
                foreach (TodoTask todosItem in list)
                {
                    if(todosItem.UserID == id)
                    sb = sb.Append("[" + todosItem.Title + "]");
                }
                return sb.ToString();
            }
            else return ("No user with ID " + id);
        }

        public List<UserToJSON> ListUsers()
        {
            var users = new List<UserToJSON>();
            var usersInDatabase = _db.Users.ToList();
            foreach (var user in usersInDatabase)
            {
                UserToJSON userToJson = new(user.Id,user.Name);
                List<TodoTask> usersTasks = _db.TodoTasks.Where(t => t.IsDone == false && t.UserID == user.Id).ToList();
                foreach(TodoTask todosItem in usersTasks)
                {
                    userToJson.tasks.Add(todosItem.Title);
                }
                users.Add(userToJson);
            }
            return users;
        }
    }
}
