using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
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

namespace Tribes.Tests.KingdomControllerTests
{
    [Serializable]
    [Collection("Serialize")]
    public class KingdomControllerTests : IntegrationTests
    {

        public KingdomControllerTests() : base("kingdomControllerTest")
        {
        }

        [Fact]
        public async Task KingdomController_Index_List()
        {
            KingdomsDTO[] expected = new KingdomsDTO[1];
            KingdomsDTO kingdom = new KingdomsDTO(1, 1, 1, "Kingdom1");
            expected[0] = kingdom;


            var response = await _client.GetAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomsDTO[]>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task KingdomController_Store_Add()
        {
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(2, 1, "memes")));

            Assert.Equal(200, (int)response.StatusCode);

        }

        [Fact]
        public async Task KingdomController_Store_Error1()
        {
            ErrorDTO expected = new ErrorDTO("User already has a kingdom");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(1, 1, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }

        [Fact]
        public async Task KingdomController_Store_Error2()
        {
            ErrorDTO expected = new ErrorDTO("Invalid world Id");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(1, 2, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Store_Error3()
        {
            ErrorDTO expected = new ErrorDTO("Invalid name");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(2, 1, "")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Store_Error4()
        {
            ErrorDTO expected = new ErrorDTO("Kingdom with this name already exists");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(2, 1, "kingdom1")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }


        [Fact]
        public async Task KingdomController_Store_Error5()
        {
            ErrorDTO expected = new ErrorDTO("Invalid user Id");
            var response = await _client.PostAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms", JsonContent.Create(new CreateKingdomDTO(5, 1, "memes")));
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());

        }

        [Fact]
        public async Task KingdomController_Show_KingdomDTO()
        {
            KingdomDTO expected = new KingdomDTO(1, 1, 1, 0, 0);

            var response = await _client.GetAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms/1");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<KingdomDTO>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(expected.Owner, outputCheck.Owner);
            Assert.Equal(expected.Id, outputCheck.Id);
            Assert.Equal(expected.CoordinateY, outputCheck.CoordinateY);
            Assert.Equal(expected.CoordinateX, outputCheck.CoordinateX);
            Assert.Equal(expected.World, outputCheck.World);
        }

        [Fact]
        public async Task KingdomController_Show_Error1()
        {
            ErrorDTO expected = new ErrorDTO("Invalid kingdom Id");

            var response = await _client.GetAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms/-3");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }

        [Fact]
        public async Task KingdomController_Show_Error2()
        {
            ErrorDTO expected = new ErrorDTO("Kingdom with this Id doesn't exist");

            var response = await _client.GetAsync("https://localhost:7192/api/kingdomrestcontroller/kingdoms/2");
            var body = response.Content.ReadAsStringAsync().Result;
            var outputCheck = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal(expected.Error, outputCheck["error"].ToString());
        }
    }
}
