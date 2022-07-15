using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TribesTest;

namespace Tribes.Tests.ArmyControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class ArmyControllerIntegrationTests : IntegrationTests
    {
        public ArmyControllerIntegrationTests() : base("armyControllerTests")
        {
        }

        [Fact]
        public async Task ArmyController_GetArmies_ArmyDTOArray()
        {
            SoldierDTO soldierDTO1 = new SoldierDTO(1, 30, 30);
            SoldierDTO soldierDTO2 = new SoldierDTO(2, 30, 30);
            List<SoldierDTO> soldierDTOs = new List<SoldierDTO>();
            soldierDTOs.Add(soldierDTO1);
            soldierDTOs.Add(soldierDTO2);
            ArmyDTO armyDTO1 = new ArmyDTO(1, 1, "Defense", soldierDTOs);
            ArmyDTO armyDTO2 = new ArmyDTO(2, 1, "Attack", new List<SoldierDTO>());
            ArmyDTO[] armyDTOs = new ArmyDTO[2];
            armyDTOs[0] = armyDTO1;
            armyDTOs[1] = armyDTO2;

            var response = await _client.GetAsync("api/armies/kingdom/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(outputCheck[0].Units.Count, 2);
            Assert.Equal(outputCheck[1].Units.Count, 0);
        }

        [Fact]
        public async Task ArmyController_GetArmy_Error1()
        {
            var response = await _client.GetAsync("api/armies/-3");
            var body =await response.Content.ReadAsStringAsync();
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
            SoldierDTO soldierDTO1 = new SoldierDTO(1, 30, 30);
            SoldierDTO soldierDTO2 = new SoldierDTO(2, 30, 30);
            List<SoldierDTO> soldierDTOs = new List<SoldierDTO>();
            soldierDTOs.Add(soldierDTO1);
            soldierDTOs.Add(soldierDTO2);
            ArmyDTO armyDTO1 = new ArmyDTO(1, 1, "Defense", soldierDTOs);

            var response = await _client.GetAsync("api/armies/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ArmyDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            Assert.Equal(outputCheck.Units[0].TotalHP, 30);
            Assert.Equal(outputCheck.Units[1].CurrentHP, 30);
        }

        [Fact]
        public async Task ArmyController_StoreArmy_Error1()
        {
            var response = await _client.PostAsync("api/armies/1", JsonContent.Create(new CreateArmyDTO(new List<int>() { 1, 2, 14, })));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 403);
            Assert.Equal(outputCheck.Error, "Unit does not belong to this kingdom");
        }

        [Fact]
        public async Task ArmyController_StoreArmy_Error2()
        {
            var response = await _client.PostAsync("api/armies/1", JsonContent.Create(new CreateArmyDTO(new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 3 })));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Maximum number of units reached");
        }

        [Fact]
        public async Task ArmyController_StoreArmy_Error3()
        {
            var response = await _client.PostAsync("api/armies/1", JsonContent.Create(new CreateArmyDTO(new List<int>() { 1,2,3,4,4,5})));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 403);
            Assert.Equal(outputCheck.Error, "Request contains duplicate soldiers");
        }

        [Fact]
        public async Task ArmyController_StoreArmy_Create()
        {
            var response = await _client.PostAsync("api/armies/1", JsonContent.Create(new CreateArmyDTO(new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 })));
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal((int)response.StatusCode, 200);
        }

        [Fact]
        public async Task ArmyController_UpdateArmy_Update()
        {
            var response = await _client.PutAsync("api/armies/1", JsonContent.Create(new UpdateArmyDTO(new List<int>() { 1, 2, 4, 5, 6, 7, 8, 9 }, new List<int>())));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ResponseDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck.Status, "ok");
        }

        [Fact]
        public async Task ArmyController_UpdateArmy_Error1()
        {
            var response = await _client.PutAsync("api/armies/1", JsonContent.Create(new UpdateArmyDTO(new List<int>() { 14 }, new List<int>())));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 403);
            Assert.Equal(outputCheck.Error, "Unit does not belong to this kingdom");
        }

        [Fact]
        public async Task ArmyController_UpdateArmy_Error2()
        {
            var response = await _client.PutAsync("api/armies/1", JsonContent.Create(new UpdateArmyDTO(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }, new List<int>())));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Maximum number of units reached");
        }

        [Fact]
        public async Task ArmyController_UpdateArmy_Error3()
        {
            var response = await _client.PutAsync("api/armies/1", JsonContent.Create(new UpdateArmyDTO(new List<int>() 
            { 1, 2, 3, 4, 5, 6, 10, 11, 12, 13 }, new List<int>() { 7, 8, 9 })));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 403);
            Assert.Equal(outputCheck.Error, "Unit does not belong to this army");
        }

        [Fact]
        public async Task ArmyController_UpdateArmy_Error4()
        {
            var response = await _client.PutAsync("api/armies/1", JsonContent.Create(new UpdateArmyDTO(new List<int>()
            { 1, 2, 3 }, new List<int>() { 7,7 })));
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 403);
            Assert.Equal(outputCheck.Error, "Request contains duplicate soldiers");
        }

        [Fact]
        public async Task ArmyController_RemoveArmy_Error1()
        {
            var response = await _client.DeleteAsync("api/armies/-1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Invalid id");
        }

        [Fact]
        public async Task ArmyController_RemoveArmy_Error2()
        {
            var response = await _client.DeleteAsync("api/armies/1");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 400);
            Assert.Equal(outputCheck.Error, "Cannot delete the kingdom's defense army");
        }

        [Fact]
        public async Task ArmyController_RemoveArmy_Error3()
        {
            var response = await _client.DeleteAsync("api/armies/3");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<ErrorDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 404);
            Assert.Equal(outputCheck.Error, "Army not found");
        }


        [Fact]
        public async Task ArmyController_RemoveArmy_Delete()
        {
            var response = await _client.DeleteAsync("api/armies/2");
            var body = await response.Content.ReadAsStringAsync();
            var outputCheck = JsonSerializer.Deserialize<StatusDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal((int)response.StatusCode, 200);
            Assert.Equal(outputCheck.status, "deleted");
        }
    }
}
