using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.SoldierDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class SoldierServiceTests :IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "KingdomServiceTest").Options;
        public ApplicationContext Db;
        public Mock<IPurchaseService> PurchaseService;
        public readonly ResourceFactory ResourceFactory;
        public SoldierService SoldierService;

        public SoldierServiceTests()
        {
            PurchaseService = new Mock<IPurchaseService>();
            Db = new ApplicationContext(options);
            ResourceFactory = new ResourceFactory();
            SoldierService = new SoldierService(Db, PurchaseService.Object);
            User user = new User();
            user.Name = "test";
            user.Email = "test";
            user.PasswordHash = "test";
            user.VerificationToken = "test";
            user.ForgottenPasswordToken = "token";
            user.Role = "Player";
            Db.Users.Add(user);
            World world = new World() { Name = "world" };
            Db.Worlds.Add(world);
            Location location = new Location();
            location.XCoordinate = 0;
            location.YCoordinate = 0;
            Db.Locations.Add(location);
            Kingdom kingdom = new Kingdom();
            kingdom.Location = location;
            kingdom.User = user;
            kingdom.WorldId = world.Id;
            kingdom.World = world;
            kingdom.UserId = user.Id;
            kingdom.Name = "kingdom";
            kingdom.Armies = new List<Army>();
            kingdom.Resources = new List<Resource>();
            Gold gold = new Gold();
            gold.Amount = 100;
            kingdom.Resources.Add(gold);
            Db.Resources.Add(gold);
            Food food = new Food();
            food.Amount = 100;
            kingdom.Resources.Add(food);
            Db.Resources.Add(food);
            Wood wood = new Wood();
            wood.Amount = 100;
            kingdom.Resources.Add(wood);
            Db.Resources.Add(wood);
            People people = new People();
            people.Amount = 100;
            Soldier soldier = new Soldier();
            soldier.Level = 2;
            kingdom.Resources.Add(soldier);
            Db.Resources.Add(soldier);
            kingdom.Resources.Add(people);
            Db.Resources.Add(people);
            Db.Kingdoms.Add(kingdom);
            Db.SaveChanges();
        }

        [Fact]
        public void SoldierService_CreateSoldiers_Error1() 
        {
            Assert.False(SoldierService.CreateSoldiers(1, new CreateSoldiersDTO(new List<int> { 0, 2 })));
            Assert.Equal("No enough units of level 2", SoldierService.GetError());
        }

        [Fact]
        public void SoldierService_CreateSoldiers_Error2()
        {
            Assert.False(SoldierService.CreateSoldiers(1, new CreateSoldiersDTO(new List<int> { 10, 1 })));
            Assert.Equal("Not enough resources", SoldierService.GetError());
        }

        [Fact]
        public void SoldierService_CreateSoldiers_Create()
        {
            Assert.True(SoldierService.CreateSoldiers(1, new CreateSoldiersDTO(new List<int> { 1, 1 })));
            Assert.Equal(2, Db.Resources.Where(r => r is Soldier).Count());
        }

        public void Dispose()
        {
            Db.Database.EnsureDeleted();
            Db.Database.EnsureCreated();
        }
    }
}
