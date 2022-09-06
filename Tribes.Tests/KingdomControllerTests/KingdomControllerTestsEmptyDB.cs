using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.KingdomControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerTestsEmptyDB : IntegrationTests
    {

        public KingdomControllerTestsEmptyDB() : base("")
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async Task KingdomControllerEmpty_Index_List()
        {
            KingdomsDTO[] expected = new KingdomsDTO[0];

            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomsDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(expected, outputCheck);
        }

        [Fact]
        public async Task KingdomControllerEmpty_Show_Error()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/1");

            Assert.Equal(404, (int)response.StatusCode);
        }

        [Fact]
        public async Task KingdomControllerEmpty_Store_Error()
        {
            var response = await _client.PostAsync("https://localhost:7192/api/kingdoms/", JsonContent.Create(new CreateKingdomDTO(1, 1, "test")));

            Assert.Equal(400, (int)response.StatusCode);
        }

        [Fact]
        public async Task KingdomControllerEmpty_GetResources_Error()
        {
            var response = await _client.GetAsync("https://localhost:7192/api/kingdoms/resources/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(400, (int)response.StatusCode);
        }
    }
}
