using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.UserTests
{
    public class KingdomControllerCreateKingdomIntegrationTests : IntegrationTests
    {
        private static string Worlds = "userControllerTestWorlds1";

        public KingdomControllerCreateKingdomIntegrationTests() : base(Worlds)
        {
        }

      

        [Fact]
        public async void Kingdom_create_by_existing_coordinates()
        {
            var expected = new { Error = "This Place has been occupied" };
            await _client.PostAsync("/api/kingdomrestcontroller/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom1", 1, 10, 10)));
            var response = _client.PostAsync("/api/kingdomrestcontroller/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(2, "Kingdom2", 1, 10, 10))).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(409, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }

        [Fact]
        public async void Kingdom_create_by_existing_user_with_kingdom()
        {
            var expected = new { Error = "User already has a kingdom" };
            await _client.PostAsync("/api/kingdomrestcontroller/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom1", 1, 10, 10)));
            var response = _client.PostAsync("/api/kingdomrestcontroller/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom2", 1, 0, 00))).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(409, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }
    }
}
