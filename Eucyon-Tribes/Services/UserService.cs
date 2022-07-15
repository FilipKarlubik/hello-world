﻿using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Eucyon_Tribes.Models.UserModels;

namespace Eucyon_Tribes.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationContext _db;
        private readonly IKingdomService _kingdomService;
        private readonly IAuthService _authService;

        public UserService(ApplicationContext db, IKingdomService kingdomService, IAuthService authService)
        {
            _db = db;
            _kingdomService = kingdomService;
            _authService = authService;
        }

        public Dictionary<int, string> CreateUser(UserCreateDto user, string kingdomName, int worldId)
        {
            if (user.Name == null || user.Name.Equals(string.Empty))
            {
                return new Dictionary<int, string> { { 400, "Username is required" } };
            }
            if (user.Password == null || user.Password.Equals(string.Empty))
            {
                return new Dictionary<int, string> { { 400, "Password is required" } };
            }
            if (user.Email == null || user.Email.Equals(string.Empty))
            {
                return new Dictionary<int, string> { { 400, "Email is required" } };
            }
            if (_db.Worlds.ToList().Count == 0)
            {
                return new Dictionary<int, string> { { 500, "No worlds in database" } };
            }
            if (_db.Users.Any(u => u.Name.Equals(user.Name)))
            {
                return new Dictionary<int, string> { { 409, "Username is already taken" } };
            }
            else if (_db.Users.Any(u => u.Email.Equals(user.Email)))
            {
                return new Dictionary<int, string> { { 409, "Email is already taken"} };
            }
            if (user.Name.Length < 4)
            {
                return new Dictionary<int, string> { { 400, "Username must be at least 4 characters long" } };
            }
            if (user.Password.Length < 8)
            {
                return new Dictionary<int, string> { { 400, "Password must be at least 8 characters long"} };
            }
            if (!validateEmail(user.Email))
            {
                return new Dictionary<int, string> { { 400, "Invalid email" } };
            }
            else
            {
                try
                {
                    User newUser = new User() { Name = user.Name, PasswordHash = user.Password, Email = user.Email };
                    SetDefaultValues(newUser);
                    _db.Users.Add(newUser);
                    _db.SaveChanges();
                }
                catch
                {
                    return new Dictionary<int, string> { { 400, "Unknown error" } };
                }

                Dictionary<int, string> result;
                if (kingdomName != null && kingdomName != "")
                {
                    if (worldId == 0)
                    {
                        Random rand = new Random();
                        int total = _db.Worlds.Count();
                        worldId = _db.Worlds.Skip(rand.Next(total)).First().Id;
                    }
                    if (_kingdomService.AddKingdom(new CreateKingdomDTO(_db.Users.FirstOrDefault(u => u.Name.Equals(user.Name)).Id, worldId, kingdomName)))
                    {
                        result = new Dictionary<int, string> { { 201, "New user " + user.Name + " created with kingdom " + kingdomName } };
                    }
                    else
                    {
                        result = new Dictionary<int, string> { { 201, "Kingdom can not be created, user was created without kingdom, you can create kingdom manually" } };
                    }
                }
                else
                {
                    result = new Dictionary<int, string> { { 201, "New user " + user.Name + " created" } };
                }
                var createdUser = _db.Users.FirstOrDefault(u => u.Email.Equals(user.Email));
                if (createdUser == null)
                {
                    return new Dictionary<int, string> { { 400, "Unknown error" } };
                }
                createdUser.VerificationToken = _authService.GenerateToken(createdUser, "verify");
                _db.SaveChanges();
                return result;
            }
        }

        private bool validateEmail(string email)
        {
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void SetDefaultValues(User user)
        {
            if (user.ForgottenPasswordToken == null) user.ForgottenPasswordToken = "";
            if (user.VerificationToken == null) user.VerificationToken = "";
        }

        public string DeleteUser(string name, string password)
        {
            if (name == null || password == null)
            {
                return "Invalid input parametters";
            }
            if (_db.Users.Any(u => u.Name.Equals(name)))
            {
                User user = _db.Users.FirstOrDefault(u => u.Name.Equals(name));
                if (user.PasswordHash.Equals(password))
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();
                    return "User " + name + " has been removed from database";
                }
                else
                {
                    return "Password does not match, user " + name + " can not be deleted";
                }
            }
            else
            {
                return "User with name " + name + " does not exist in database";
            }
        }

        public bool DestroyUser(int id, string password)
        {
            if (password == null)
            {
                return false;
            }
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return false;
            }
            else if (!user.PasswordHash.Equals(password))
            {
                return false;
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            return true;
        }

        public bool EditUser(int id, string name, string password)
        {
            if (name == null || password == null)
            {
                return false;
            }
            User user = _db.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null)
            {
                return false;
            }
            if (!user.PasswordHash.Equals(password))
            {
                return false;
            }
            user.Name = name;
            _db.Users.Update(user);
            _db.SaveChanges();
            return true;
        }

        public List<UserResponseDto> ListAllUsers(int page, int itemCount)
        {
            if (itemCount < 1) itemCount = 20;
            if (page < 1) page = 1;
            int totalCount = _db.Users.Count();
            if (totalCount < page * itemCount)
            {
                if (totalCount % itemCount == 0) page = totalCount / itemCount;
                else page = totalCount / itemCount + 1;
            }
            List<User> usersInDB = _db.Users.OrderByDescending(u => u.Id).Skip((page - 1) * itemCount).Take(itemCount).ToList();
            List<UserResponseDto> users = new();
            foreach (User user in usersInDB)
            {
                UserResponseDto u = new(
                user.Id,
                user.Name,
                user.VerifiedAt
                );
                users.Add(u);
            }
            return users;
        }

        public string Login(UserLoginDto login)
        {
            if (login.Name == null || login.Password == null)
            {
                return "Invalid input parametters";
            }
            if (_db.Users.Any(u => u.Name.Equals(login.Name)))
            {
                User user = _db.Users.FirstOrDefault(u => u.Name.Equals(login.Name));
                if (user.PasswordHash.Equals(login.Password))
                {
                    user.VerifiedAt = DateTime.Now;
                    _db.Users.Update(user);
                    _db.SaveChanges();
                    return "User " + login.Name + " logged in";
                }
                else
                {
                    return "User " + login.Name + " wrong password";
                }
            }
            else
            {
                return "User " + login.Name + " is not in database";
            }
        }

        public UserResponseDto ShowUser(int id)
        {
            if (!_db.Users.Any(u => u.Id.Equals(id)))
            {
                return null;
            }
            User userFromDB = _db.Users.FirstOrDefault(u => u.Id.Equals(id));
            UserResponseDto user = new UserResponseDto(
                userFromDB.Id,
                userFromDB.Name,
                userFromDB.VerifiedAt);

            return user;
        }

        public int StoreUsers(UsersInputDto users)
        {
            int count = 0;
            foreach (UserCreateDto user in users.users)
            {
                if (!_db.Users.Any(x => x.Email.Equals(user.Email)) ||
                    !_db.Users.Any(x => x.Name.Equals(user.Name)))
                {
                    Console.WriteLine(CreateUser(user, null, 0));
                    count++;
                }
            }
            return count;
        }

        public bool UpdateUser(int id, UserCreateDto user)
        {
            User u = _db.Users.FirstOrDefault(u => u.Id == id);
            if (u == null)
            {
                return false;
            }
            if (!u.PasswordHash.Equals(user.Password))
            {
                return false;
            }
            u.Name = user.Name;
            u.Email = user.Email;
            _db.Users.Update(u);
            _db.SaveChanges();
            return true;
        }

        public User UserInfo(string name)
        {
            if (name == null)
            {
                return null;
            }
            if (_db.Users.Any(u => u.Name.Equals(name)))
            {
                User user = _db.Users.FirstOrDefault(u => u.Name.Equals(name));
                user.PasswordHash = "**********";
                return user;
            }
            else return null;
        }

        public List<UserDetailDto> UsersInfoDetailedForAdmin(string adminPass)
        {
            string adminPassword = "guess_what";

            if (adminPass != adminPassword) return null;

            List<Kingdom> kingdoms = _db.Kingdoms.ToList();
            List<User> users = _db.Users.ToList();
            List<Location> locations = _db.Locations.ToList();

            List<UserDetailDto> userDetailed = new();
            foreach (User user in users)
            {
                Kingdom k = new();
                if (kingdoms.Any(k => k.UserId.Equals(user.Id)))
                {
                    k = kingdoms.FirstOrDefault(k => k.UserId.Equals(user.Id));
                }
                Location l = new();
                if (locations.Any(x => x.KingdomId.Equals(k.Id)))
                {
                    l = locations.FirstOrDefault(l => l.KingdomId.Equals(k.Id));
                }
                UserDetailDto u = new(user.Id, user.Name, user.Email, user.PasswordHash, k.Id, l.Id
                        , l.XCoordinate, l.YCoordinate, user.CreatedDate, user.VerifiedAt, k.WorldId, k.Name
                        , user.VerificationToken, user.ForgottenPasswordToken);
                userDetailed.Add(u);
            }
            return userDetailed;
        }
    }
}
