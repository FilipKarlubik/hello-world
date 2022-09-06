using Eucyon_Tribes.Context;
using Eucyon_Tribes.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Tribes.Services
{
    public class TimeService : BackgroundService
    {
        public int tick;
        public System.Timers.Timer timer;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimeService(IConfiguration config, IServiceScopeFactory serviceScopeFactory)
        {
            _config = config;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private void CreateServices()
        {
            var scope = _serviceScopeFactory.CreateScope();
            var resourceService = scope.ServiceProvider.GetService<IResourceService>();
            resourceService.UpdateResource();
            resourceService.FamineCheck();
            var battleService = scope.ServiceProvider.GetService<IBattleService>();
            battleService.CheckForBattles();
            var armyService = scope.ServiceProvider.GetService<IArmyService>();
            armyService.RemoveArmy();
        }

        public void OnTickEvent(Object source, ElapsedEventArgs t)
        {
            Console.WriteLine("game tick occured");
            CreateServices();
            tick++;
        }

        protected override Task ExecuteAsync(System.Threading.CancellationToken stoppingToken)
        {
            timer = new System.Timers.Timer
            {
                Interval = int.Parse(Environment.GetEnvironmentVariable("TribesGametickLength")) * 1000,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += OnTickEvent;
            return Task.CompletedTask;
        }
    }
}
