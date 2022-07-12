using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace TribesTest
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomServiceTest : IDisposable
    {
        private readonly static DbContextOptions options = new DbContextOptionsBuilder<ApplicationContext>()
               .UseInMemoryDatabase(databaseName: "KingdomServiceTest").Options;
        public ApplicationContext Context;
        public KingdomService Service;
        public KingdomFactory Factory;
        public ResourceFactory ResourceFactory;
        public BuildingFactory BuildingFactory;

        public KingdomServiceTest()
        {
            Context = new ApplicationContext(options);
            ResourceFactory = new ResourceFactory();
            BuildingFactory = new BuildingFactory();
            Factory = new KingdomFactory(Context, ResourceFactory, BuildingFactory);
            Service = new KingdomService(Context, Factory);
            User User = new User();
            User.Name = "test";
            User.Email = "test";
            User.PasswordHash = "test";
            User.ForgottenPasswordToken = "test";
            User.VerificationToken = "test";
            Context.Users.Add(User);
            User User2 = new User();
            User2.Name = "test";
            User2.Email = "test";
            User2.PasswordHash = "test";
            User2.ForgottenPasswordToken = "test";
            User2.VerificationToken = "test";
            Context.Users.Add(User2);
            User User3 = new User();
            User3.Name = "test";
            User3.Email = "test";
            User3.PasswordHash = "test";
            User3.ForgottenPasswordToken = "test";
            User3.VerificationToken = "test";
            Context.Users.Add(User3);
            World World = new World();
            Context.Worlds.Add(World);
            Context.SaveChanges();
        }
        public void Dispose()
        {
            foreach (var kingdom in Context.Kingdoms)
                Context.Kingdoms.Remove(kingdom);
            foreach (var world in Context.Worlds)
                Context.Worlds.Remove(world);   
            foreach (var user in Context.Users)
                Context.Users.Remove(user);
            foreach (var location in Context.Locations)
                Context.Locations.Remove(location);
            Context.SaveChanges();
        }

        [Fact]
        public void KingdomServiceTest_AddKingdom_Add()
        {
            CreateKingdomDTO test = new CreateKingdomDTO(Context.Users.First().Id, Context.Worlds.First().Id, "memes");
            Service.AddKingdom(test);

            Assert.True(Context.Kingdoms.ToList().Count() == 1);
        }

        [Fact]
        public void KingdomServiceTest_AddKingdom_NullFullWorld()
        {
            List<Kingdom> kingdoms = new List<Kingdom>();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Kingdom kingdom = new Kingdom();
                    Location location = new Location();
                    location.XCoordinate = i;
                    location.YCoordinate = j;
                    kingdom.Location = location;
                    kingdom.Name = "test";
                    kingdom.World = Context.Worlds.First();
                    Context.Kingdoms.Add(kingdom);

                }
            }
            Context.SaveChanges();

            Service.AddKingdom(new CreateKingdomDTO(Context.Users.First().Id, Context.Worlds.First().Id, "test"));

            Assert.True(Context.Kingdoms.ToList().Count() == 240);

        }

        [Fact]
        public void KingdomServiceTest_GetKingdoms_KingdomList()
        {
            Kingdom kingdom1 = new Kingdom();
            Kingdom kingdom2 = new Kingdom();
            Kingdom kingdom3 = new Kingdom();
            List<Kingdom> kingdoms = new List<Kingdom> { kingdom1, kingdom2, kingdom3 };
            KingdomsDTO[] kingdomsDTO = new KingdomsDTO[3];
            for (int i = 0; i < 3; i++)
            {
                kingdoms[i].User = Context.Users.ToList()[i];
                kingdoms[i].UserId = Context.Users.ToList()[i].Id;
                kingdoms[i].Name = "test";
                kingdoms[i].World = Context.Worlds.First();
                kingdoms[i].WorldId = Context.Worlds.First().Id;
                Context.Kingdoms.Add(kingdoms[i]);
                Context.SaveChanges();
                KingdomsDTO kingdomDTO = new KingdomsDTO(kingdoms[i].Id, Context.Worlds.First().Id
                    , kingdoms[i].UserId, kingdoms[i].Name);
                kingdomsDTO[i] = kingdomDTO;
            }

            Assert.Equal(kingdomsDTO.Count(), Service.GetKingdoms().Count());
            Assert.Equal(kingdomsDTO[0].Name, Service.GetKingdoms()[0].Name);
            Assert.Equal(kingdomsDTO[1].Owner, Service.GetKingdoms()[1].Owner);
            Assert.Equal(kingdomsDTO[2].World, Service.GetKingdoms()[2].World);
        }

        [Fact]
        public void KingdomServiceTest_GetKingdom_Null()
        { 
            Assert.Null(Service.GetKindom(0));
        }

        [Fact]
        public void KingdomServiceTest_GetKingdom_KingdomDTO()
        {
            Kingdom kingdom1 = new Kingdom();
            kingdom1.User = Context.Users.ToList()[0];
            kingdom1.UserId = Context.Users.ToList()[0].Id;
            kingdom1.Name = "test";
            kingdom1.World = Context.Worlds.First();
            kingdom1.WorldId = Context.Worlds.First().Id;
            Kingdom kingdom = new Kingdom();
            Location location = new Location();
            location.XCoordinate = 0;
            location.YCoordinate = 0;
            kingdom1.Location = location;
            Context.Locations.Add(location);
            Context.Kingdoms.Add(kingdom1);
            Context.SaveChanges();
            KingdomDTO kingdomDTO = new KingdomDTO(kingdom1.Id, kingdom1.WorldId = Context.Worlds.First().Id,
                Context.Users.ToList()[0].Id, Context.Locations.First().XCoordinate,
                Context.Locations.First().YCoordinate);

            Assert.Equal(Service.GetKindom(Context.Kingdoms.First().Id).World,kingdomDTO.World);
            Assert.Equal(Service.GetKindom(Context.Kingdoms.First().Id).Owner, kingdomDTO.Owner);
            Assert.Equal(Service.GetKindom(Context.Kingdoms.First().Id).CoordinateX, kingdomDTO.CoordinateX);
            Assert.Equal(Service.GetKindom(Context.Kingdoms.First().Id).CoordinateY, kingdomDTO.CoordinateY);
            Assert.Equal(Service.GetKindom(Context.Kingdoms.First().Id).Id, kingdomDTO.Id); 
        }

    }
}
