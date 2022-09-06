using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.WorldDTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TribesTest;

namespace Tribes.Tests.WorldTests
{
    [Serializable]
    [Collection("Serialize")]
    public class WorldControllerTestsWithoutWorlds : IntegrationTests
    {
        
        public WorldControllerTestsWithoutWorlds() : base("worldControllerWithoutWorldsTest")
        {
        }

        [Fact]
        public async void IndexTest()
        {
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7192/api/worlds");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            var expected = new WorldResponseDTO[0];
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<WorldResponseDTO[]>(body);
            Assert.Equal(500, (int)response.StatusCode);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async void CreateTest_WithEmptyName()
        {

            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7192/api/worlds/create?name=");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given name is unsuitable.", result.Error);
        }

        [Fact]
        public async void CreateTest_WithCorrectName()
        {
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7192/api/worlds/create?name=Sikastan");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task StoreTest_WithWrongDTO()
        {
            var world = new StoreWorldDTO(String.Empty, new List<Kingdom>(), new List<Location>());
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var content = JsonContent.Create(world);
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, "https://localhost:7192/api/worlds");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = content;
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given world cannot be stored.", result.Error);
        }

        [Fact]
        public async Task StoreTest_WithCorrectDTO()
        {
            var world = new StoreWorldDTO("Sikastan", new List<Kingdom>(), new List<Location>());
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var content = JsonContent.Create(world);
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Post, "https://localhost:7192/api/worlds");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = content;
            var response = await _client.SendAsync(requestMessage);     
            Assert.Equal(201, (int)response.StatusCode);
        }

        [Fact]
        public async Task ShowTest()
        {
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7192/api/worlds/1");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Given ID was not found.", result.Error);
        }

        [Fact]
        public async Task EditTest()
        {
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:7192/api/worlds/1/edit");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be edited.", result.Error);
        }

        [Fact]
        public async Task UpdateTest()
        {
            var world = new UpdateWorldDTO("Sikastan");
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var content = JsonContent.Create(world);
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Put, "https://localhost:7192/api/worlds/1");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Content = content;
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be updated.", result.Error);
        }

        [Fact]
        public async Task DestroyTest()
        {
            var token = "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6ImZmZmZmQGdqZ2Znby5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiRmlsaXAiLCJleHAiOjE2NTkxNzIzNzR9.5o0heGQ4RmmADHc0aT_9IEvtJ8_cBafBtZ5Qkf5aRGk";
            var requestMessage =
            new HttpRequestMessage(HttpMethod.Delete, "https://localhost:7192/api/worlds/1");
            requestMessage.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(requestMessage);
            var body = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ErrorDTO>(body);
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("World with given ID could not be destroyed.", result.Error);
        }
    }
}
