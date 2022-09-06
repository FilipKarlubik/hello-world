using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.TimeServiceTests
{
    [Serializable]
    [Collection("Serialize")]
    public class TimeServiceIntegrationTests : IntegrationTests
    {
        public TimeServiceIntegrationTests() : base("timeService")
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async void TimeService_ResourceUpdate_Update()
        {
            await Task.Delay(10500);

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/resources/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ResourcesDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.True(outputCheck.Resources[0].Amount >= 9);
            Assert.True(outputCheck.Resources[0].Amount < 35);
        }

        [Fact]
        public async void TimeService_ArmyRemove_Update()
        {
            await Task.Delay(10000);

            var response = await _client.GetAsync("https://localhost:7192/api/armies/kingdom/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(1, outputCheck.Length);
            Assert.Equal(1, outputCheck[0].NumberOfUnitsByLevel[1]);
        }

        [Fact]
        public async void TimeService_CheckForBattles_Update()
        {
            await Task.Delay(10000);

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/battles/kingdom/1/1/2");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<List<BattleResposeDto>>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(2, outputCheck.Count());
            Assert.True(outputCheck[0].Fought_at > DateTime.Now);
            Assert.Equal(1, outputCheck[1].AttackerKingdomId);
            Assert.Equal(1, outputCheck[0].AttackerKingdomId);
            Assert.Equal("kingdom", outputCheck[1].Winner);
            Assert.Equal("Battle not yet fought", outputCheck[0].Winner);
        }

        [Fact]
        public async void TimeService_FamineCheck_Update()
        {
            await Task.Delay(10000);

            var response = await _client.GetAsync("https://localhost:7192/api/armies/kingdom/availableunits/2");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<AvailableUnitsDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(0, outputCheck.NumberOfUnitsByLevel.Count());
        }
    }
}
