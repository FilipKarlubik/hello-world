using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tribes.Services;
using TribesTest;

namespace Tribes.Tests
{
    [Serializable]
    [Collection("Serialize")]
    public class TimeServiceTest
    {
        public IConfiguration Config;
        public TimeServiceTest()
        {
            Config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        }

        [Fact]
        public async Task TimeService_Tick()
        {
            TimeService timeService = new TimeService(Config);
            timeService.StartAsync(new CancellationToken());
            timeService.timer.Interval = 1000;
            StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            await Task.Delay(10000);
            
            string consoleOutput = stringWriter.ToString();
            String[] strings = consoleOutput.Split("\n");
            Assert.Equal(10, timeService.tick);
            Assert.Equal(11, strings.Length);
            Assert.Equal(strings[9], "game tick occured\r");
        }
    }
}
