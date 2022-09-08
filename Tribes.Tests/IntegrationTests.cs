using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tribes.Tests.SeedData;

namespace TribesTest
{ 
    public class IntegrationTests
    {
        protected readonly HttpClient _client;
        protected readonly IAuthService authService;
        protected readonly IConfiguration configuration;

        protected IntegrationTests(String dataSeed)
        {
            configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>().Build();
            foreach (var child in configuration.GetChildren())
            {
                if (dataSeed == "timeService" && child.Key.Equals("TRIBESGAMETICKLEN"))
                {
                    child.Value="5";
                }
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            } 
            authService = new JWTService();

            var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(host =>
            {
                host.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                        typeof(DbContextOptions<ApplicationContext>));

                    services.Remove(descriptor);
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDB");
                    });
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var appDb = scopedServices.GetRequiredService<ApplicationContext>();
                    appDb.Database.EnsureDeleted();
                    appDb.Database.EnsureCreated();

                        switch (dataSeed)
                        {
                            case "kingdomControllerTest":
                                SeedDataKingdomController.PopulateForKingdomControllerTest(appDb);
                                break;
                                
                            case "buildingControllerTests":
                                BuildingControllerTestsSeedData.PopulateTestData(appDb);
                                break;

                            case "armyControllerTests":
                                SeedDataArmyController.PopulateDataForArmyControllerTest(appDb);
                                break;

                            
                            case "buildingControllerTestsNoResource":
                                BuildingControllerTestsSeedDataNoResources.PopulateTestData(appDb);
                                break;

                            case "userControllerTestWorlds0":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;

                            case "userControllerTestWorlds1":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;

                            case "leaderboardControllerWithKingdomsTest":
                                SeedDataLeaderboardController.PopulateTestData(appDb, true);
                                break;

                            case "leaderboardControllerWithoutKingdomsTest":
                                SeedDataLeaderboardController.PopulateTestData(appDb, false);
                                break;

                            case "worldControllerWithoutWorldsTest":
                                SeedDataWorldController.PopulateTestData(appDb, "0");
                                break;

                            case "worldControllerWithOneWorldTest":
                                SeedDataWorldController.PopulateTestData(appDb, "1");
                                break;

                            case "worldControllerWithMultipleWorldsTest":
                                SeedDataWorldController.PopulateTestData(appDb, "3");
                                break;

                            case "emailControllerTests":                                
                                EmailControllerTestSeedData.PopulateTestData(appDb);
                                var user = appDb.Users.FirstOrDefault();
                                user.VerificationToken = authService.GenerateToken(user, "verify");                         
                                var user1 = appDb.Users.FirstOrDefault(p => p.Id == 2);
                                user1.ForgottenPasswordToken = authService.GenerateToken(user1, "forgotten password");
                                var user2 = appDb.Users.FirstOrDefault(p => p.Id == 3);
                                user2.ForgottenPasswordToken = authService.GenerateToken(user1, "forgotten password");
                                appDb.SaveChanges();
                                break;

                            case "timeService":
                                SeedDataTimeService.PopulateForTimeService(appDb);
                                break;

                            default:
                                break;
                        }    
                });
            });
            _client = appFactory.CreateClient();
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJQbGF5ZXIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJmaWxpcC5mZmZrYXJsdWJpa0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiSHVydmluZWsiLCJleHAiOjE2NjI2NzEzNDJ9.LTilu4yRmYQ-Hr_Gxx_Kh8mCbha9UNyMjunFUH-uvOY";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }
    }
}