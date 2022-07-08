using Eucyon_Tribes.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Tribes.Tests.SeedData;

namespace TribesTest
{
    public class IntegrationTests
    {
        protected readonly HttpClient _client;

        protected IntegrationTests(String dataSeed)
        {
            var appFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(ApplicationContext));
                    services.AddDbContext<ApplicationContext>(options =>
                    {
                        options.UseInMemoryDatabase(databaseName: "IntegrationTests");
                    });
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
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
                            
                            case "userControllerTestWorlds0":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;

                            case "userControllerTestWorlds1":
                                UserSeedData.PopulateTestData(appDb, dataSeed);
                                break;
                            
                            default:
                                break;
                        }
                    }
                });
            });
            _client = appFactory.CreateClient();
        }
    }
}