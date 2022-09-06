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
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerCreateKingdomIntegrationTests : IntegrationTests
    {
        private static string Worlds = "userControllerTestWorlds1";

        public KingdomControllerCreateKingdomIntegrationTests() : base(Worlds)
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async void Kingdom_create_by_valid_input()
        {
            var expected = new { Status = "Kingdom created" };
            var response = await _client.PostAsync("/api/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom1", 1, 10, 10)));
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(201, (int)response.StatusCode);
            Assert.Equal(expected.Status, result["status"].ToString());
        }

        [Fact]
        public async void Kingdom_create_by_existing_coordinates()
        {
            var expected = new { Error = "This Place has been occupied" };
            await _client.PostAsync("/api/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom1", 1, 10, 10)));
            var response = _client.PostAsync("/api/kingdoms/with_location"
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
            await _client.PostAsync("/api/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom1", 1, 10, 10)));
            var response = _client.PostAsync("/api/kingdoms/with_location"
                , JsonContent.Create(new KingdomCreateRequestDTO(1, "Kingdom2", 1, 0, 00))).Result;
            var body = response.Content.ReadAsStringAsync().Result;
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(409, (int)response.StatusCode);
            Assert.Equal(expected.Error, result["error"].ToString());
        }
    }
}
