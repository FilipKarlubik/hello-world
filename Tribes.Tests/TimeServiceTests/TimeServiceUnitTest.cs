using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribes.Services;
using TribesTest;

namespace Tribes.Tests.TimeServiceTests
{
    [Serializable]
    [Collection("Serialize")]
    public class TimeServiceTest :IDisposable
    {
        public Mock<IBattleService> BattleService;
        public Mock<IArmyService> ArmyService;
        public IConfiguration Config;
        public ApplicationContext ApplicationContext;
        private readonly static DbContextOptions Options = new DbContextOptionsBuilder<ApplicationContext>()
              .UseInMemoryDatabase(databaseName: "KingdomServiceTest").Options;
        public Mock<IServiceProvider> ServiceProvider;
        public Mock<IServiceScope> ServiceScope;
        public Mock<IServiceScopeFactory> ServiceScopeFactory;
        public TimeServiceTest()
        {
            Config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
            foreach (var child in Config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
            ApplicationContext = new ApplicationContext(Options);
            ServiceProvider = new Mock<IServiceProvider>();
            ServiceScope = new Mock<IServiceScope>();
            ServiceScopeFactory = new Mock<IServiceScopeFactory>();
            BattleService = new Mock<IBattleService>();
            ArmyService = new Mock<IArmyService>();
        }

        [Fact]
        public async Task TimeService_Tick()
        {
            ServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(ServiceScopeFactory.Object);
            ServiceScopeFactory.Setup(x => x.CreateScope()).Returns(ServiceScope.Object);
            ServiceScope.Setup(x => x.ServiceProvider).Returns(ServiceProvider.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IResourceService))).Returns(new ResourceService(ApplicationContext));
            TimeService timeService = new TimeService(Config, ServiceScopeFactory.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IBattleService))).Returns(BattleService.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IArmyService))).Returns(ArmyService.Object);
            timeService.StartAsync(new CancellationToken());
            timeService.timer.Interval = 1000;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            await Task.Delay(10500);

            string consoleOutput = stringWriter.ToString();
            string[] strings = consoleOutput.Split("\n");
            if (strings[9].EndsWith("\r")) strings[9] = strings[9].Substring(0, strings[9].Length - 1);
            Assert.True(timeService.tick>=9);
            Assert.True(strings.Length>=10);
            //Assert.True(strings[strings.Length-2].Equals("game tick occured") || strings[strings.Length - 2].Equals("game tick occured\r"));
        }

        [Fact]
        public async Task TimeService_DbUpdate()
        {
            World world = new World
            {
                Name = "world",
                Kingdoms = new List<Kingdom>(),
                Locations = new List<Location>()
            };
            User user1 = new User
            {
                Name = "user",
                Email = "email",
                PasswordHash = "password",
                ForgottenPasswordToken = "token",
                VerificationToken = "token"
            };
            user1.Role = "Player";
            Location location1 = new Location
            {
                World = world,
                XCoordinate = 0,
                YCoordinate = 0
            };
            Kingdom kingdom1 = new Kingdom
            {
                Name = "kingdom",
                World = world,
                Location = location1,
                Armies = new List<Army>(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                User = user1,
                AttackBattles = new List<Battle>()
            };
            Farm farm = new Farm
            {
                Kingdom = kingdom1,
                Production = 60,
                Level = 1
            };
            Food food = new Food
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            Wood wood = new Wood
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            Gold gold = new Gold
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            People people = new People
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            kingdom1.Resources.Add(food);
            kingdom1.Resources.Add(wood);
            kingdom1.Resources.Add(gold);
            kingdom1.Resources.Add(people);
            kingdom1.Buildings.Add(farm);
            ServiceProvider.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(ServiceScopeFactory.Object);
            ServiceScopeFactory.Setup(x => x.CreateScope()).Returns(ServiceScope.Object);
            ServiceScope.Setup(x => x.ServiceProvider).Returns(ServiceProvider.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IResourceService))).Returns(new ResourceService(ApplicationContext));
            ServiceProvider.Setup(x => x.GetService(typeof(IBattleService))).Returns(BattleService.Object);
            ServiceProvider.Setup(x => x.GetService(typeof(IArmyService))).Returns(ArmyService.Object);
            TimeService timeService = new TimeService(Config, ServiceScopeFactory.Object);
            timeService.StartAsync(new CancellationToken());
            timeService.timer.Interval = 5000;
            ApplicationContext.Worlds.Add(world);
            ApplicationContext.Kingdoms.Add(kingdom1);
            ApplicationContext.Users.Add(user1);
            ApplicationContext.Locations.Add(location1);
            ApplicationContext.Resources.Add(food);
            ApplicationContext.Resources.Add(wood);
            ApplicationContext.Resources.Add(gold);
            ApplicationContext.Resources.Add(people);
            ApplicationContext.Buildings.Add(farm);
            ApplicationContext.SaveChanges();

            await Task.Delay(10500);
            timeService = null;

            List<Resource> resources = ApplicationContext.Resources.ToList();
            Food foodCheck =(Food)(resources.FirstOrDefault(r => r.GetType() == typeof(Food)));

            Assert.True(foodCheck.Amount>=9);
            Assert.True(foodCheck.Amount < 15);
        }

        public void Dispose()
        {
            ApplicationContext.Database.EnsureDeleted();
            ApplicationContext.Database.EnsureCreated();
        }
    }
}
