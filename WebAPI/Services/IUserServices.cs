using WebAPI.Models;

namespace WebAPI.Services
{
    public interface IUserServices
    {
        string AddUser(string name);
        public List<UserToJSON> ListUsers();
        string GetTodosById(int id);
    }
}
