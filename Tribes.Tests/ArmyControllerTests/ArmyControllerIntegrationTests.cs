﻿using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.ArmyControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class ArmyControllerIntegrationTests : IntegrationTests
    {
        public ArmyControllerIntegrationTests() : base("armyControllerTests")
        {
            var accessToken = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }

        [Fact]
        public async Task ArmyController_GetArmies_ArmyDTOArray()
        {
            var response = await _client.GetAsync("api/armies/kingdom/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck[0].Id, 1);
            Assert.Equal(outputCheck[0].Owner, 1);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel.Count(), 2);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel[0], 6);
            Assert.Equal(outputCheck[0].NumberOfUnitsByLevel[1], 6);
            Assert.Equal(outputCheck[1].Id, 2);
            Assert.Equal(outputCheck[1].Owner, 1);
            Assert.Equal(outputCheck[1].NumberOfUnitsByLevel.Count(), 0);
        }

        [Fact]
        public async Task ArmyController_GetArmy_Error1()
        {
            var response = await _client.GetAsync("api/armies/-3");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Invalid id");
        }

        [Fact]
        public async Task ArmyController_GetArmy_Error2()
        {
            var response = await _client.GetAsync("api/armies/10");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 404);
            Assert.Equal(outputCheck.Error, "Army not found");
        }

        [Fact]
        public async Task ArmyController_GetArmy_ArmyDTO()
        {
            ArmyDTO expected = new ArmyDTO(1, 1, new List<int> { 6, 6 });

            var response = await _client.GetAsync("api/armies/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck.Id, expected.Id);
            Assert.Equal(outputCheck.Owner, expected.Owner);
            Assert.Equal(outputCheck.NumberOfUnitsByLevel.Count(), expected.NumberOfUnitsByLevel.Count());
            Assert.Equal(outputCheck.NumberOfUnitsByLevel[0], expected.NumberOfUnitsByLevel[0]);
            Assert.Equal(outputCheck.NumberOfUnitsByLevel[1], expected.NumberOfUnitsByLevel[1]);
        }

        [Fact]
        public async Task ArmyController_AvailableUnits_Empty()
        {
            var response = await _client.GetAsync("api/armies/kingdom/availableunits/3");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<AvailableUnitsDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(0, outputCheck.NumberOfUnitsByLevel.Count());
        }

        [Fact]
        public async Task ArmyController_AvailableUnits_DTO()
        {
            var response = await _client.GetAsync("api/armies/kingdom/availableunits/2");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<AvailableUnitsDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(0, outputCheck.NumberOfUnitsByLevel[0]);
            Assert.Equal(6, outputCheck.NumberOfUnitsByLevel[1]);
        }

        [Fact]
        public async Task ArmyController_UnitsUnderConstruction_DTO()
        {
            var response = await _client.GetAsync("api/armies/kingdom/unitsunderconstruction/2");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<UnitsUnderConstructionDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(6, outputCheck[0].NumberOfSoldiersByLevel[0]);
        }

        [Fact]
        public async Task ArmyController_UnitsUnderConstruction_Empty()
        {
            var response = await _client.GetAsync("api/armies/kingdom/unitsunderconstruction/3");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<UnitsUnderConstructionDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.True(outputCheck.Length == 0);
        }
    }
}
