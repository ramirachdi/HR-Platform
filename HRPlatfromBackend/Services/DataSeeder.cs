using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjetNET.Controllers;
using ProjetNET.Models;
using ProjetNET.TestData;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetNET.Services
{
    public class DataSeeder : IHostedService
    {
        private readonly ILogger<DataSeeder> _logger;
        private readonly IServiceProvider _serviceProvider;

        public DataSeeder(ILogger<DataSeeder> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MyService is starting.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Your data seeding logic using the scoped context
                var fakeUsers = new UserGenerator().GenerateFakeUsers(50);
                scopedContext.Users.AddRange(fakeUsers);

                var fakeEquipes = new ParentEquipeGenerator(scopedContext).GenerateFakeEquipes(5);
                scopedContext.Equipes.AddRange(fakeEquipes);



                await scopedContext.SaveChangesAsync(cancellationToken);
            }
        }

        public System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("MyService is stopping.");

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
}


