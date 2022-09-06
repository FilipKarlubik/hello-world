using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Tribes.Tests.ResourceTests
{
    [Serializable]
    [Collection("Serialize")]
    public class ResourceServiceTest
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "ResourceServiceTest").Options;
        public ApplicationContext _db;
        public ResourceService Service;

        public ResourceServiceTest()
        {
            _db = new ApplicationContext(options);
            Service = new ResourceService(_db);
            SeedDb();
        }

        public void SeedDb()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();
            DateTime timestamp = DateTime.Now + new TimeSpan(0, -2, 0);

            var sawMill = new Sawmill() { Level = 1, Production = 10 };
            var mine = new Mine() { Level = 1, Production = 10 };
            var townHall = new TownHall() { Level = 1, Production = 10 };
            var farm = new Farm() { Level = 1, Production = 10 };
            var wood = new Wood() { Amount = 0, UpdatedAt = timestamp };
            var gold = new Gold() { Amount = 0, UpdatedAt = timestamp };
            var food = new Food() { Amount = 0, UpdatedAt = timestamp };
            var people = new People() { Amount = 0, UpdatedAt = timestamp };
            var soldier = new Soldier() { Amount = 0, UpdatedAt = timestamp };
            List<Building> buildings = new() { sawMill, mine, townHall, farm };
            List<Resource> resources = new() { wood, gold, food, people, soldier };
            var kingdom = new Kingdom() { Name = "Aden", Buildings = buildings, Resources = resources };

            _db.Kingdoms.Add(kingdom);
            _db.SaveChanges();
        }

        [Fact]
        public void SeedDbForResourcesTest()
        {
            var resources = _db.Kingdoms.Include(k => k.Resources).First().Resources;
            Assert.Equal(5, resources.Count);
            foreach (var resource in resources)
            {
                Assert.Equal(0, resource.Amount);
            }
        }

        [Fact]
        public void SeedDbForBuildingsTest()
        {
            var buildings = _db.Kingdoms.Include(k => k.Buildings).First().Buildings;
            Assert.Equal(4, buildings.Count);
            foreach (var building in buildings)
            {
                Assert.Equal(1, building.Level);
                Assert.Equal(10, building.Production);
            }
        }

        [Fact]
        public void UpdateResourceTest()
        {
            Service.UpdateResource();
            var kingdom = _db.Kingdoms.Include(k => k.Resources).First();
            foreach (var resource in kingdom.Resources)
            {
                if (resource is Food)
                {
                    Assert.Equal(18, resource.Amount);
                }
                else if (resource is not Soldier)
                {
                    Assert.Equal(20, resource.Amount);
                }
            }
        }
        [Fact]
        public void UpdateResourceKingdomTest()
        {
            Service.UpdateResourceKingdom(1);
            var kingdom = _db.Kingdoms.Include(k => k.Resources).First();
            foreach (var resource in kingdom.Resources)
            {
                if (resource is Food) 
                {
                    Assert.Equal(18, resource.Amount);
                }
                else if (resource is not Soldier)
                {
                    Assert.Equal(20, resource.Amount);
                }
            }
        }

        [Fact]
        public void ResourceService_FamineCheck_Famine()
        {
            World world = new World
            {
                Name = "world",
                Kingdoms = new List<Kingdom>(),
                Locations = new List<Location>()
            };
            User user = new User
            {
                Name = "user",
                Email = "email",
                PasswordHash = "password",
                ForgottenPasswordToken = "token",
                VerificationToken = "token"
            };
            user.Role = "Player";
            Location location = new Location
            {
                World = world,
                XCoordinate = 0,
                YCoordinate = 0
            };
            Kingdom kingdom = new Kingdom
            {
                Name = "kingdom",
                World = world,
                Location = location,
                Armies = new List<Army>(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                User = user,
                AttackBattles = new List<Battle>()
            };
            Soldier soldier = new Soldier();
            Food food = new Food
            {
                UpdatedAt = DateTime.Now,
                Amount = -1
            };
            kingdom.Resources.Add(food);
            kingdom.Resources.Add(soldier);
            _db.Worlds.Add(world);
            _db.Users.Add(user);
            _db.Locations.Add(location);
            _db.Kingdoms.Add(kingdom);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.SaveChanges();

            Service.FamineCheck();

            Assert.Equal(49,food.Amount);
            Assert.Null(_db.Resources.FirstOrDefault(r => r.Kingdom == kingdom && r is Soldier));
        }

        [Fact]
        public void ResourceService_FamineCheck_Ok()
        {
            World world = new World
            {
                Name = "world",
                Kingdoms = new List<Kingdom>(),
                Locations = new List<Location>()
            };
            User user = new User
            {
                Name = "user",
                Email = "email",
                PasswordHash = "password",
                ForgottenPasswordToken = "token",
                VerificationToken = "token"
            };
            user.Role = "Player";
            Location location = new Location
            {
                World = world,
                XCoordinate = 0,
                YCoordinate = 0
            };
            Kingdom kingdom = new Kingdom
            {
                Name = "kingdom",
                World = world,
                Location = location,
                Armies = new List<Army>(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                User = user,
                AttackBattles = new List<Battle>()
            };
            Soldier soldier = new Soldier();
            Food food = new Food
            {
                UpdatedAt = DateTime.Now,
                Amount = 1
            };
            kingdom.Resources.Add(food);
            kingdom.Resources.Add(soldier);
            _db.Worlds.Add(world);
            _db.Users.Add(user);
            _db.Locations.Add(location);
            _db.Kingdoms.Add(kingdom);
            _db.Resources.Add(food);
            _db.Resources.Add(soldier);
            _db.SaveChanges();

            Service.FamineCheck();

            Assert.Equal(1, food.Amount);
            Assert.NotNull(_db.Resources.FirstOrDefault(r => r.Kingdom == kingdom && r is Soldier));
        }
    }
}
