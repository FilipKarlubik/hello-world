using Eucyon_Tribes.Models.DTOs.LeaderboardDTOs;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.LeaderboardTests
{
    [Serializable]
    [Collection("Serialize")]
    public class LeaderboardControllerWithoutKingdomsTests : IntegrationTests
    {
        public LeaderboardControllerWithoutKingdomsTests() : base("leaderboardControllerWithoutKingdomsTest")
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async void GetLeaderboardByBuildingsWithoutKingdomsTest()
        {
            var response = await _client.GetAsync("api/leaderboard/buildings");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<BuildingLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(new List<BuildingScoreDTO>(), result.Leaderboard);
        }

        [Fact]
        public async void GetLeadeboardBySoldiersWithoutKingdomTest()
        {
            var response = await _client.GetAsync("api/leaderboard/soldiers");
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SoldierLeaderboardDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(new List<SoldierScoreDTO>(), result.Leaderboard);
        }
    }
}
