using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.LeaderboardTests
{
    [Serializable]
    [Collection("Serialize")]
    public class LeaderboardControllerWithKingdomsTests : IntegrationTests
    {
        public LeaderboardControllerWithKingdomsTests() : base("leaderboardControllerWithKingdomsTest")
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async void GetLeaderboardByBuildingsTest()
        {
            int[][] expected = new int[][] { new int[] { 10, 6 }, new int[] { 10, 2 }, new int[] { 5, 1 } };
            var response = await _client.GetAsync("api/leaderboard/buildings");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BuildingLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(200, (int)response.StatusCode);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], result.Leaderboard[i].BuildingScore);
            }
        }

        [Fact]
        public async void GetLeadeboardBySoldiersTest()
        {
            int[] expected = new int[] { 250, 150, 100 };
            var response = await _client.GetAsync("api/leaderboard/soldiers");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SoldierLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(200, (int)response.StatusCode);
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(expected[i], result.Leaderboard[i].SoldierScore);
            }
        }
    }
}
