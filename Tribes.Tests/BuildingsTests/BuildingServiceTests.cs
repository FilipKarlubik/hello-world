using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Tribes.Tests.BuildingsTests
{
    public class BuildingServiceTests
    {
        private readonly DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName: "BuildingsTests").Options;
        private ApplicationContext _db;
        private BuildingService _buildingService;

        public BuildingServiceTests()
        {
            _db = new ApplicationContext(options);
            _buildingService = new BuildingService(_db);
        }

        [Fact]
        public void SavingABuildingTest()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            TownHall townHall = new TownHall();
            _buildingService.SaveBuilding(townHall);

            Assert.Equal(1, _db.Buildings.Count());
            Assert.NotEqual(2, _db.Buildings.Count());
        }

        [Fact]
        public void GetBuildingByIdTest()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var buildings = new List<Building>()
            {
                new TownHall(),
                new Farm(),
                new Mine()
            };

            _db.Buildings.AddRange(buildings);
            _db.SaveChanges();

            Assert.Equal(buildings[0], _buildingService.GetBuildingById(1));
        }

        public void SeedDatabase()
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() { Email = "john@john.com", PasswordHash = "Johny123", Name = "John", ForgottenPasswordToken = "", VerificationToken = "" };
            _db.Users.Add(user);

            var kingdom = new Kingdom() { Name = "Aden" };
            _db.Kingdoms.Add(kingdom);

            user.Kingdom = kingdom;

            var mine = new Mine()
            {
                Level = 1,
                Hp = 100,
                StartedAt = DateTime.Today,
                Production = 7,
            };
            var sawmill = new Sawmill()
            {
                Level = 1,
                Hp = 75,
                StartedAt = DateTime.Today,
                Production = 15,
            };
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);

            var gold = new Gold();
            var wood = new Wood();
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Resources = new List<Resource>();
            kingdom.Resources.Add(gold);
            kingdom.Resources.Add(wood);
            _db.SaveChanges();
        }

        [Fact]
        public void GetAllBuldingsOfKingdomTest()
        {
            SeedDatabase();

            List<BuildingsResponseDto> buildingsResponseDtos = new List<BuildingsResponseDto>();

            var productionDto = new ProductionDto(
                1,
                "Gold",
                0,
                false,
                DateTime.Today,
                DateTime.Today);

            var buildingsResponseDto = new BuildingsResponseDto(
                1,
                1,
                "Mine",
                1,
                productionDto);

            buildingsResponseDtos.Add(buildingsResponseDto);

            var productionDto2 = new ProductionDto(
                2,
                "Wood",
                0,
                false,
                DateTime.Today,
                DateTime.Today);

            var buildingsResponseDto2 = new BuildingsResponseDto(
                2,
                1,
                "Sawmill",
                1,
                productionDto2);

            buildingsResponseDtos.Add(buildingsResponseDto2);

            Assert.Equal(buildingsResponseDtos[0].Production.Resource, _buildingService.GetAllBuldingsOfKingdom(1)[0].Production.Resource);
            Assert.Equal(buildingsResponseDtos[0].Production.Amount, _buildingService.GetAllBuldingsOfKingdom(1)[0].Production.Amount);
            Assert.Equal(buildingsResponseDtos[0].Production.Started_at, _buildingService.GetAllBuldingsOfKingdom(1)[0].Production.Started_at);
            Assert.Equal(buildingsResponseDtos[0].Kingdom, _buildingService.GetAllBuldingsOfKingdom(1)[0].Kingdom);
            Assert.Equal(buildingsResponseDtos[0].Type, _buildingService.GetAllBuldingsOfKingdom(1)[0].Type);
            Assert.Equal(buildingsResponseDtos[0].Level, _buildingService.GetAllBuldingsOfKingdom(1)[0].Level);
            Assert.Equal(buildingsResponseDtos[1].Production.Resource, _buildingService.GetAllBuldingsOfKingdom(1)[1].Production.Resource);
            Assert.Equal(buildingsResponseDtos[1].Production.Amount, _buildingService.GetAllBuldingsOfKingdom(1)[1].Production.Amount);
            Assert.Equal(buildingsResponseDtos[1].Production.Started_at, _buildingService.GetAllBuldingsOfKingdom(1)[1].Production.Started_at);
            Assert.Equal(buildingsResponseDtos[1].Kingdom, _buildingService.GetAllBuldingsOfKingdom(1)[1].Kingdom);
            Assert.Equal(buildingsResponseDtos[1].Type, _buildingService.GetAllBuldingsOfKingdom(1)[1].Type);
            Assert.Equal(buildingsResponseDtos[1].Level, _buildingService.GetAllBuldingsOfKingdom(1)[1].Level);
            Assert.Equal("WrongUser", _buildingService.GetAllBuldingsOfKingdom(8)[0].Type);
        }

        [Fact]
        public void SetResourceTypeTest()
        {
            SeedDatabase();

            Assert.Equal("Gold", _buildingService.SetResourceType(1, "Mine").GetType().Name);
            Assert.Equal("Wood", _buildingService.SetResourceType(1, "Sawmill").GetType().Name);
        }

        [Fact]
        public void GetBuildingDetailsTest()
        {
            SeedDatabase();

            var productionDto = new ProductionDto(
                1,
                "Gold",
                0,
                false,
                DateTime.Today,
                DateTime.Today);

            var levelingCostDto = new LevelingCostDto(0, 0);

            var buildingResponseDto = new BuildingResponseDto(
                1,
                1,
                "Mine",
                1,
                productionDto,
                levelingCostDto);

            Assert.Equal(buildingResponseDto.Kingdom, _buildingService.GetBuildingDetails(1, 1).Kingdom);
            Assert.Equal(buildingResponseDto.Type, _buildingService.GetBuildingDetails(1, 1).Type);
            Assert.Equal(buildingResponseDto.Level, _buildingService.GetBuildingDetails(1, 1).Level);
            Assert.Equal(buildingResponseDto.Production.Resource, _buildingService.GetBuildingDetails(1, 1).Production.Resource);
            Assert.Equal(buildingResponseDto.Production.Amount, _buildingService.GetBuildingDetails(1, 1).Production.Amount);
            Assert.Equal(buildingResponseDto.Production.Collected, _buildingService.GetBuildingDetails(1, 1).Production.Collected);
            Assert.Equal(buildingResponseDto.Leveling_Cost.Gold, _buildingService.GetBuildingDetails(1, 1).Leveling_Cost.Gold);
            Assert.Equal("WrongUser", _buildingService.GetBuildingDetails(1, 8).Type);
            Assert.Equal("WrongBuilding", _buildingService.GetBuildingDetails(8, 1).Type);
        }

        [Fact]
        public void DeleteBuildingByIdTest()
        {
            SeedDatabase();

            _buildingService.DeleteBuildingById(1, 1);

            Assert.Equal("Sawmill", _db.Buildings.FirstOrDefault().GetType().Name);
            Assert.Equal("WrongBuilding", _buildingService.DeleteBuildingById(5, 1).Status);
            Assert.Equal("WrongUser", _buildingService.DeleteBuildingById(2, 5).Status);
        }

        [Fact]
        public void StoreNewBuldingTest()
        {
            SeedDatabase();

            BuildingRequestDto buildingRequestDto = new BuildingRequestDto(1);
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == 1);
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };

            _buildingService.StoreNewBulding(buildingRequestDto, 1);

            Assert.Equal(barracks.Level, kingdom.Buildings[2].Level);
            Assert.Equal(barracks.Hp, kingdom.Buildings[2].Hp);
            Assert.Equal(barracks.Production, kingdom.Buildings[2].Production);

            BuildingRequestDto wrongBuilding = new BuildingRequestDto(6);
            Assert.Equal("WrongBuilding", _buildingService.StoreNewBulding(wrongBuilding, 1).Status);
            Assert.Equal("WrongUser", _buildingService.StoreNewBulding(buildingRequestDto, 5).Status);
        }

        [Fact]
        public void CreateRightBuildingTest()
        {
            _db.Database.EnsureCreated();
            Barracks barracks = new Barracks()
            {
                Level = 1,
                Hp = 300,
                StartedAt = DateTime.Today,
                Production = 1,
            };
            var barracks1 = _buildingService.CreateRightBuilding(1);

            Assert.Equal(barracks.Hp, barracks1.Hp);
            Assert.Equal(barracks.Level, barracks1.Level);
            Assert.Equal(barracks.Production, barracks1.Production);
        }
    }
}