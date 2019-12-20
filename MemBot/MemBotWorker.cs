using System;
using System.Threading;
using System.Threading.Tasks;
using MemBotModels.ServicePrototypes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MemBotWorker
{
    public class MemBotWorker : BackgroundService
    {
        private readonly ILogger<MemBotWorker> _logger;
        private readonly MemBotClient _memBot;

        public MemBotWorker(ILogger<MemBotWorker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;

            using var scope = serviceScopeFactory.CreateScope();
            IMemService memService = scope.ServiceProvider.GetService<IMemService>();

            _memBot = new MemBotClient(memService);
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // TODO: Configure logger to log into EventLog and text file or use another one
            _logger.LogInformation("MemBotWorker is started", DateTime.Now);
            _memBot.Start();

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _memBot.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}
