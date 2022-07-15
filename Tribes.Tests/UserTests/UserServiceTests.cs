using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tribes.Tests.UserTests
{
    [Serializable]
    [Collection("Serialize")]
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
        public IAuthService authService;
        public ArmyFactory armyFactory;

        public UserServiceTest()
        {

            DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
                 .UseInMemoryDatabase(databaseName: "UsersList").Options;
            db = new ApplicationContext(options);
            resourceFactory = new ResourceFactory();
            buildingFactory = new BuildingFactory();
            armyFactory = new ArmyFactory();
            kingdomFactory = new KingdomFactory(db, resourceFactory, buildingFactory);
            var config = new ConfigurationBuilder().AddUserSecrets("5ea770c2-4c16-4659-94eb-5a89323b961c").Build();
            authService = new JWTService(config);
            kingdomService = new KingdomService(db, kingdomFactory,armyFactory);
            userService = new UserService(db, kingdomService, authService);

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

            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
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

            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Fact]
        public void Delete_Non_Existing_User_Test()
        {
            var expected = 2;
            userService.DeleteUser("Izonka", "i");

            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Fact]
        public void Delete_Existing_User_With_Wrong_Pass_Test()
        {
            var expected = 2;
            userService.DeleteUser("Klotilda", "i");

            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Fact]
        public void Create_User_Test()
        {
            var expected = 3;
            UserCreateDto user = new("Izonka", "i12345678", "izonka@gmail.com");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Fact]
        public void Create_Existing_User_Test()
        {
            var expected = 2;
            UserCreateDto user = new("Matilda", "m12345678", "");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Fact]
        public void Create_User_With_Existing_Email_Test()
        {
            var expected = 2;
            UserCreateDto user = new("Izonka", "i12345678", "matilda@gmail.com");
            userService.CreateUser(user, null, 0);
            Assert.Equal(expected, userService.ListAllUsers(0,0).Count);
        }

        [Theory]
        [InlineData(null, "12345678", "mail@gmail.com", 400)]
        [InlineData("abcde", null, "mail@gmail.com", 400)]
        [InlineData("abcde", "12345678", null, 400)]
        [InlineData("abc", "12345678", "mail@gmail.com", 400)]
        [InlineData("abcdef", "1234", "mail@gmail.com", 400)]
        [InlineData("abcdef", "12345678", "mailgmail.com", 400)]
        public void CreateUserWithDifferentNullInputs(string name, string password, string email, int statusCode)
        {
            UserCreateDto user = new UserCreateDto(name, password, email);
            var response = userService.CreateUser(user, null, 1);
            Assert.Equal(statusCode, response.ElementAt(0).Key);
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