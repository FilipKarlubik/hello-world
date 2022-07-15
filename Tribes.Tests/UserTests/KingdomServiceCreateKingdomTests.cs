using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tribes.Tests.UserTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomServiceCreateKingdomTests : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "UsersList3").Options;

        public ApplicationContext db;
        public UserService userService;
        public KingdomService kingdomService;
        public KingdomFactory kingdomFactory;
        public BuildingFactory buildingFactory;
        public ResourceFactory resourceFactory;
        public IAuthService authService;
        public ArmyFactory armyFactory;

        public KingdomServiceCreateKingdomTests()
        {

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
                Name = "Matilda",
                PasswordHash = "m",
                Email = "matilda@gmail.com",
                VerificationToken = "",
                ForgottenPasswordToken = ""
            };

            db.Worlds.Add(new World());
            db.Users.Add(user1);
            db.Users.Add(user2);
            db.SaveChanges();

        }

        public void Dispose()
        {
            foreach (var user in db.Users)
                db.Users.Remove(user);
            db.SaveChanges();
        }

        [Fact]
        public void KingdomCreateWithValidCoordinatesAndUser()
        {
            User user = db.Users.First();
            World world = db.Worlds.First();
            KingdomCreateRequestDTO request = new(user.Id, "Velka Morava", world.Id, 10, 10);
            var result = kingdomService.AddKingdomWithLocation(request);
            var expected = "Kingdom created";
            Assert.Equal(expected, result.Message);
        }

        [Fact]
        public void KingdomCreateWithExistingName()
        {
            World world = db.Worlds.First();
            User user1 = db.Users.First();
            User user2 = db.Users.Last();
            KingdomCreateRequestDTO request1 = new(user1.Id, "Kingdom", world.Id, 10, 10);
            KingdomCreateRequestDTO request2 = new(user2.Id, "Kingdom", world.Id, 15, 15);
            kingdomService.AddKingdomWithLocation(request1);
            var result = kingdomService.AddKingdomWithLocation(request2);
            var expected = "Kingdom with that name already exists";
            Assert.Equal(expected, result.Message);
        }

        [Fact]
        public void KingdomCreateWithExistingCoordinates()
        {
            User user1 = db.Users.First();
            User user2 = db.Users.Last();
            World world = db.Worlds.First();
            KingdomCreateRequestDTO request1 = new(user1.Id, "Kingdom1", world.Id, 10, 10);
            KingdomCreateRequestDTO request2 = new(user2.Id, "Kingdom2", world.Id, 10, 10);
            kingdomService.AddKingdomWithLocation(request1);
            var result = kingdomService.AddKingdomWithLocation(request2);
            var expected = "This Place has been occupied";
            Assert.Equal(expected, result.Message);
        }
    }
}