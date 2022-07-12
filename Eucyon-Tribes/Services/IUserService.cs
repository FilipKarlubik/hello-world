using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.UserModels;

namespace Eucyon_Tribes.Services
{
    public interface IUserService
    {
        string Login(UserLoginDto login);
        List<UserResponseDto> ListAllUsers();
        Dictionary<int, string> CreateUser(UserCreateDto user, string kingdomName, int worldId);
        string DeleteUser(string name, string password);
        User UserInfo(string name);
        List<UserDetailDto> UsersInfoDetailedForAdmin(string adminPass);
        UserResponseDto ShowUser(int id);
        bool DestroyUser(int id, string password);
        bool EditUser(int id, string name, string password);
        bool UpdateUser(int id, UserCreateDto user);
        int StoreUsers(UsersInputDto users);      
    }
}