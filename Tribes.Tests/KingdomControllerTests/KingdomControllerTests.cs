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
    public class KingdomControllerTests : IntegrationTests
    {

        public KingdomControllerTests() : base("kingdomControllerTest")
        {
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

    }
}
