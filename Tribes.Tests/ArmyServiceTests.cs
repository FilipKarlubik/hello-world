using Eucyon_Tribes.Config;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.Resources;
using Eucyon_Tribes.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class ArmyServiceTests : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "ArmyServiceTest").Options;
        public ApplicationContext Context;
        public ArmyService ArmyService;
        public Mock<IArmyFactory> ArmyFactory;
        public IConfiguration Config;

        public ArmyServiceTests()
        {
            Config = new ConfigurationBuilder().AddUserSecrets("b7595051-3f87-49e4-8f55-9fe1dfe724d1").Build();
            Context = new ApplicationContext(options);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            ArmyFactory = new Mock<IArmyFactory>();
            ArmyService = new ArmyService(ArmyFactory.Object, Context,Config);
            User user = new User();
            user.Name = "test";
            user.Email = "test";
            user.PasswordHash = "test";
            user.VerificationToken="test";
            user.ForgottenPasswordToken = "token";
            Context.Users.Add(user);
            World world = new World();
            Context.Worlds.Add(world);
            Location location = new Location();
            location.XCoordinate = 0;
            location.YCoordinate = 0;
            Context.Locations.Add(location);
            Kingdom kingdom = new Kingdom();
            kingdom.Location = location;
            kingdom.User = user;
            kingdom.WorldId = world.Id;
            kingdom.World = world;
            kingdom.UserId = user.Id;
            kingdom.Name = "kingdom";
            Army army = new Army();
            army.Type = "Defense";
            army.Soldiers = new List<Soldier>();
            kingdom.Armies = new List<Army>();
            kingdom.Resources = new List<Resource>();
            kingdom.Armies.Add(army);
            Context.Armies.Add(army);
            Soldier soldier1 = new Soldier();
            soldier1.TotalHP = 30;
            soldier1.CurrentHP = 30;
            Soldier soldier2 = new Soldier();
            soldier2.TotalHP = 30;
            soldier2.CurrentHP = 30;
            Soldier soldier3 = new Soldier();
            soldier3.TotalHP = 30;
            soldier3.CurrentHP = 30;
            kingdom.Resources.Add(soldier1);
            kingdom.Resources.Add(soldier2);
            kingdom.Resources.Add(soldier3);
            Context.Resources.Add(soldier1);
            Context.Resources.Add(soldier2);
            Context.Resources.Add(soldier3);
            kingdom.Resources.Add(soldier1);
            Context.Kingdoms.Add(kingdom);
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        [Fact]
        public void ArmyServiceTest_GetArmies_ArmyDto()
        {
            ArmyDTO[] expected = new ArmyDTO[1];
            expected[0] = new ArmyDTO(Context.Armies.First().Id, Context.Kingdoms.First().Id, "Defense", new List<SoldierDTO>());

            ArmyDTO[] result = ArmyService.GetArmies(Context.Kingdoms.First().Id);

            Assert.Equal(expected[0].Id, result[0].Id);
            Assert.Equal(expected[0].Owner, result[0].Owner);
            Assert.Equal(expected[0].Type, result[0].Type);
            Assert.Equal(expected[0].Units, result[0].Units);
        }

        [Fact]
        public void ArmyServiceTest_GetArmies_EmptyArmyDto()
        {
            Context.Armies.Remove(Context.Armies.First());
            Context.SaveChanges();
            ArmyDTO[] expected = new ArmyDTO[0];

            ArmyDTO[] result = ArmyService.GetArmies(Context.Kingdoms.First().Id);

            Assert.Equal(expected, result);
        }


        [Fact]
        public void ArmyServiceTest_GetArmy_error1()
        {

            ArmyDTO expected = null;

            ArmyDTO result = ArmyService.GetArmy(10);

            Assert.Equal(expected, result);
            Assert.True(ArmyService.ErrorMessage.Equals("Army not found"));
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_error2()
        {

            ArmyDTO expected = null;

            ArmyDTO result = ArmyService.GetArmy(-2);

            Assert.Equal(expected, result);
            Assert.True(ArmyService.ErrorMessage.Equals("Invalid id"));
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_EmptyArmyDTO()
        {

            ArmyDTO expected = new ArmyDTO(Context.Armies.First().Id, Context.Kingdoms.First().Id, "Defense", new List<SoldierDTO>());

            ArmyDTO result = ArmyService.GetArmy(Context.Kingdoms.First().Id);

            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Owner, result.Owner);
            Assert.Equal(expected.Type, result.Type);
            Assert.Equal(expected.Units, result.Units);
        }

        [Fact]
        public void ArmyServiceTest_GetArmy_ArmyDTO()
        {
            Army army = Context.Armies.First();
            army.Soldiers.AddRange(Context.Resources.Select(r => (Soldier)r).ToList());
            Context.SaveChanges();
            List<SoldierDTO> soldiers = new List<SoldierDTO>();
            soldiers.Add(new SoldierDTO(Context.Resources.ToList()[0].Id, 30, 30));
            soldiers.Add(new SoldierDTO(Context.Resources.ToList()[1].Id, 30, 30));
            soldiers.Add(new SoldierDTO(Context.Resources.ToList()[2].Id, 30, 30));
            ArmyDTO expected = new ArmyDTO(Context.Armies.First().Id, Context.Kingdoms.First().Id, "Defense", soldiers);

            ArmyDTO result = ArmyService.GetArmy(Context.Kingdoms.First().Id);

            Assert.Equal(expected.Id, result.Id);
            Assert.Equal(expected.Owner, result.Owner);
            Assert.Equal(expected.Type, result.Type);
            Assert.Equal(expected.Units[0].Id, result.Units[0].Id);
            Assert.Equal(expected.Units[1].CurrentHP, result.Units[1].CurrentHP);
            Assert.Equal(expected.Units[2].TotalHP, result.Units[2].TotalHP);
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Error1()
        {
            User user = new User();
            user.Name = "user";
            user.PasswordHash = "password";
            user.Email = "email";
            user.VerificationToken = "token";
            user.ForgottenPasswordToken = "token";
            Location location = new Location();
            location.XCoordinate = 1;
            location.YCoordinate = 1;
            Kingdom kingdom = new Kingdom();
            kingdom.Name = "name";
            World world = new World();
            kingdom.World = world;
            kingdom.WorldId = world.Id;
            kingdom.Location = location;
            location.KingdomId = kingdom.Id;
            location.Kingdom = kingdom;
            user.Kingdom = kingdom;
            kingdom.User = user;
            kingdom.UserId = user.Id;
            kingdom.Resources = new List<Resource>();
            Soldier soldier = new Soldier();
            kingdom.Armies = new List<Army>();
            Army army = new Army();
            army.Type = "Defense";
            army.Kingdom = kingdom;
            kingdom.Armies.Add(army);
            soldier.Kingdom = kingdom;
            soldier.Army = army;
            Context.Resources.Add(soldier);
            Context.Armies.Add(army);
            Context.Users.Add(user);
            Context.Worlds.Add(world);
            Context.Locations.Add(location);
            Context.Kingdoms.Add(kingdom);
            Context.SaveChanges();
            CreateArmyDTO createArmyDTO = new CreateArmyDTO(new List<int> { soldier.Id });

            ArmyService.CreateArmy(createArmyDTO, Context.Kingdoms.Where(k => k.Id != kingdom.Id).First().Id);

            Assert.Equal(Context.Kingdoms.Include(k => k.Armies).Where(k => k.Id != kingdom.Id).First().Armies.Count(), 1);
            Assert.True(ArmyService.ErrorMessage.Equals("Unit does not belong to this kingdom"));
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Error2()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            List<int> soldiers = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                Soldier soldier = new Soldier();
                kingdom.Resources.Add(soldier);
                Context.Resources.Add(soldier);
                soldiers.Add(soldier.Id);
            }
            Context.SaveChanges();
            CreateArmyDTO createArmyDTO = new CreateArmyDTO(soldiers);

            ArmyService.CreateArmy(createArmyDTO, kingdom.Id);

            Assert.Equal(Context.Kingdoms.Include(k => k.Armies).First().Armies.Count(), 1);
            Assert.True(ArmyService.ErrorMessage.Equals("Maximum number of units reached"));
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Error3()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            Soldier soldier1 = new Soldier();
            kingdom.Resources.Add(soldier1);
            Context.Resources.Add(soldier1);
            soldier1.KingdomId = kingdom.Id;
            Context.SaveChanges();
            CreateArmyDTO createArmyDTO = new CreateArmyDTO(new List<int> { 1, 1 });

            ArmyService.CreateArmy(createArmyDTO,Context.Armies.First().Id);

            Assert.True(Context.Armies.First().Soldiers.Count == 0);
            Assert.Equal(ArmyService.ErrorMessage, "Request contains duplicate soldiers");
        }

        [Fact]
        public void ArmyServiceTest_CreateArmy_Add()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            List<int> soldiers = new List<int>();
            List<Soldier> soldierList = new List<Soldier>();
            for (int i = 0; i < 5; i++)
            {
                Soldier soldier = new Soldier();
                kingdom.Resources.Add(soldier);
                Context.Resources.Add(soldier);
                soldiers.Add(soldier.Id);
                soldierList.Add(soldier);
            }
            Context.SaveChanges();
            CreateArmyDTO createArmyDTO = new CreateArmyDTO(soldiers);
            Army army = new Army();
            army.Type = "Attack";
            army.Soldiers = soldierList;
            ArmyFactory.Setup(s => s.CrateArmy(soldierList, kingdom)).Returns(army);

            Assert.True(ArmyService.CreateArmy(createArmyDTO, kingdom.Id));
            Assert.Equal(Context.Kingdoms.Include(k => k.Armies).First().Armies.Count(), 2);
        }

        [Fact]
        public void ArmyServiceTest_UpdateArmy_Error1()
        {
            UpdateArmyDTO updateArmyDTO = new UpdateArmyDTO(new List<int> { 0, 1, 100 }, new List<int> { });

            ArmyService.UpdateArmy(Context.Armies.First().Id, updateArmyDTO);

            Assert.True(Context.Armies.First().Soldiers.Count == 0);
            Assert.Equal(ArmyService.ErrorMessage, "Unit does not belong to this kingdom");
        }


        [Fact]
        public void ArmyServiceTest_UpdateArmy_Error2()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            List<int> soldiers = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                Soldier soldier = new Soldier();
                kingdom.Resources.Add(soldier);
                Context.Resources.Add(soldier);
                soldier.KingdomId = kingdom.Id;
                soldiers.Add(soldier.Id);
            }
            Context.SaveChanges();
            UpdateArmyDTO updateArmyDTO = new UpdateArmyDTO(soldiers, new List<int> { });

            ArmyService.UpdateArmy(Context.Armies.First().Id, updateArmyDTO);

            Assert.True(Context.Armies.First().Soldiers.Count == 0);
            Assert.Equal(ArmyService.ErrorMessage, "Maximum number of units reached");
        }

        [Fact]
        public void ArmyServiceTest_UpdateArmy_Error3()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            Soldier soldier1 = new Soldier();
            kingdom.Resources.Add(soldier1);
            Context.Resources.Add(soldier1);
            soldier1.KingdomId = kingdom.Id;
            List<int> soldiers = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                Soldier soldier = new Soldier();
                soldier.KingdomId=kingdom.Id;
                soldier.Kingdom = kingdom;
                kingdom.Resources.Add(soldier);
                Context.Resources.Add(soldier);
                soldiers.Add(soldier.Id);
            }
            Context.SaveChanges();
            UpdateArmyDTO updateArmyDTO = new UpdateArmyDTO(soldiers, new List<int> {1});

            ArmyService.UpdateArmy(Context.Armies.First().Id, updateArmyDTO);

            Assert.True(Context.Armies.First().Soldiers.Count == 0);
            Assert.Equal(ArmyService.ErrorMessage, "Unit does not belong to this army");
        }

        [Fact]
        public void ArmyServiceTest_UpdateArmy_Error4()
        {
            Kingdom kingdom = Context.Kingdoms.First();
            Soldier soldier1 = new Soldier();
            kingdom.Resources.Add(soldier1);
            Context.Resources.Add(soldier1);
            soldier1.KingdomId = kingdom.Id;
            Context.SaveChanges();
            UpdateArmyDTO updateArmyDTO = new UpdateArmyDTO(new List<int> {1,1 }, new List<int>());

            ArmyService.UpdateArmy(Context.Armies.First().Id, updateArmyDTO);

            Assert.True(Context.Armies.First().Soldiers.Count == 0);
            Assert.Equal(ArmyService.ErrorMessage, "Request contains duplicate soldiers");
        }
        [Fact]
        public void ArmyServiceTest_UpdateArmy_Update()
        {
            Army army = Context.Armies.First();
            Soldier soldier1 = new Soldier();
            Kingdom kingdom = Context.Kingdoms.First();
            kingdom.Resources.Add(soldier1);
            Context.Resources.Add(soldier1);
            army.Soldiers.Add(soldier1);
            List<int> soldiers = new List<int>();
            List<int> toRemove = new List<int>();
            for (int i = 0; i < 12; i++)
            {
                Soldier soldier = new Soldier();
                kingdom.Resources.Add(soldier);
                Context.Resources.Add(soldier);
                soldier.Kingdom = kingdom;
                soldier.KingdomId = kingdom.Id;
                soldiers.Add(soldier.Id);
                if (i == 11)
                {
                    soldier.Army = army;
                    toRemove.Add(soldier.Id);
                }
            }
            Context.SaveChanges();
            UpdateArmyDTO updateArmyDTO = new UpdateArmyDTO(soldiers, toRemove);

            ArmyService.UpdateArmy(Context.Armies.First().Id, updateArmyDTO);
            Assert.Equal(12, Context.Armies.First().Soldiers.Count);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Error1() 
        {
            ArmyService.RemoveArmy(-9);

            Assert.True(Context.Armies.ToList().Count == 1);
            Assert.Equal("Invalid id", ArmyService.ErrorMessage);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Error2()
        {
            ArmyService.RemoveArmy(100);

            Assert.True(Context.Armies.ToList().Count == 1);
            Assert.Equal("Army not found", ArmyService.ErrorMessage);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Error3()
        {
            ArmyService.RemoveArmy(Context.Armies.First().Id);

            Assert.True(Context.Armies.ToList().Count == 1);
            Assert.Equal("Cannot delete the kingdom's defense army", ArmyService.ErrorMessage);
        }

        [Fact]
        public void ArmyServiceTest_RemoveArmy_Remove()
        {
            Army army=Context.Armies.First();
            army.Type = "Attack";
            Context.SaveChanges();
            ArmyService.RemoveArmy(Context.Armies.First().Id);

            Assert.True(Context.Armies.ToList().Count == 0);
        }
    }
}
