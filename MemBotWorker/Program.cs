using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemBotDataAccess;
using MemBotDataAccess.Services;
using MemBotModels.ServicePrototypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MemBotWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<MemBotDbContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"),
                            assembly => assembly.MigrationsAssembly(typeof(MemBotDbContext).Assembly.FullName));
                    });

                    var serviceProvider = services.BuildServiceProvider();
                    services.AddScoped<IMemService, MemService>(provider =>
                    {
                        MemBotDbContext memBotDbContext = serviceProvider.GetService<MemBotDbContext>();
                        return new MemService(memBotDbContext);
                    });
                    services.AddHostedService<MemBotWorker>();
                });
        }
    }
}
