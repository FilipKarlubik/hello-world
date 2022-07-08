using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.UserModels;
using Eucyon_Tribes.Services;
using Microsoft.EntityFrameworkCore;

namespace Tribes.Tests.UserTests
{
    public class UserServiceTest : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersList").Options;

        public ApplicationContext db;
        public UserService userService;
        public KingdomService kingdomService;
        public KingdomFactory kingdomFactory;
        public BuildingFactory buildingFactory;
        public ResourceFactory resourceFactory;

        public UserServiceTest()
        {

            DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
                 .UseInMemoryDatabase(databaseName: "UsersList").Options;
            db = new ApplicationContext(options);
            resourceFactory = new ResourceFactory();
            buildingFactory = new BuildingFactory();
            kingdomFactory = new KingdomFactory(db, resourceFactory, buildingFactory);
            kingdomService = new KingdomService(db, kingdomFactory);
            userService = new UserService(db, kingdomService);


            var user1 = new User()
            {
                Name = "Matilda",
                PasswordHash = "m",
                Email = "matilda@gmail.com",
                VerificationToken = "",
                ForgottenPasswordToken = ""
            };

            var user2 = new User()
            {
                Name = "Klotilda",
                PasswordHash = "k",
                Email = "klotilda@gmail.com",
                VerificationToken = "",
                ForgottenPasswordToken = ""
            };

            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Worlds.Add(new World());
            db.SaveChanges();

        }

        public void Dispose()
        {
            foreach (var user in db.Users)
                db.Users.Remove(user);

            db.SaveChanges();
        }

        [Fact]
        public void ListAllUsersTest()
        {
            var expected = 2;

            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Login_Existing_User_Test()
        {
            var expected = "User Matilda logged in";
            UserLoginDto user = new("Matilda", "m");
            Assert.Equal(expected, userService.Login(user));
        }

        [Fact]
        public void Login_Non_Existing_User_Test()
        {
            var expected = "User Pipi is not in database";
            UserLoginDto user = new("Pipi", "m");
            Assert.Equal(expected, userService.Login(user));
        }

        [Fact]
        public void Login_Existing_User_With_Wrong_Pass_Test()
        {
            var expected = "User Klotilda wrong password";
            UserLoginDto user = new("Klotilda", "lol");
            Assert.Equal(expected, userService.Login(user));
        }

        [Fact]
        public void Delete_Existing_User_Test()
        {
            var expected = 1;
            userService.DeleteUser("Klotilda", "k");

            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Delete_Non_Existing_User_Test()
        {
            var expected = 2;
            userService.DeleteUser("Izonka", "i");

            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Delete_Existing_User_With_Wrong_Pass_Test()
        {
            var expected = 2;
            userService.DeleteUser("Klotilda", "i");

            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Create_User_Test()
        {
            var expected = 3;
            UserCreateDto user = new("Izonka", "i", "izonka@gmail.com");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Create_Existing_User_Test()
        {
            var expected = 2;
            UserCreateDto user = new("Matilda", "m", "");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void Create_User_With_Existing_Email_Test()
        {
            var expected = 2;
            UserCreateDto user = new("Izonka", "i", "matilda@gmail.com");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers().Count);
        }

        [Fact]
        public void User_Info_Non_Existing_User_Test()
        {
            User expected = null;

            Assert.Equal(expected, userService.UserInfo("Izonka"));
        }

        [Fact]
        public void User_Info_Existing_User_Test()
        {
            Assert.IsType<User>(userService.UserInfo("Matilda"));
        }

        [Fact]
        public void User_Show_Existing_User_Data()
        {
            string expexted = "Matilda";
            int id = db.Users.FirstOrDefault(u => u.Name.Equals("Matilda")).Id;
            Assert.IsType<UserResponseDto>(userService.ShowUser(id));

            Assert.Equal(expexted, userService.ShowUser(id).Username);
        }

        [Fact]
        public void User_Show_Non_Existing_User_Data()
        {
            int id = 0;
            UserResponseDto expected = null;

            Assert.Equal(expected, userService.ShowUser(id));
        }
    }
}