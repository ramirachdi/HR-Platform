using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjetNET.Controllers;

public class DeadlineChecker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<DeadlineChecker> _logger;

    public DeadlineChecker(IServiceProvider services, ILogger<DeadlineChecker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected async override System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (true)
        {
            using (var scope = _services.CreateScope())
            {
                Console.WriteLine("check deadline");
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>(); // Replace YourDbContext with your actual DbContext

                // Fetch and process entities with deadlines
                var entitiesWithDeadlines = dbContext.Tasks.Where(e => e.DeadLine <= DateTime.Now && e.Status != "Validé");
                foreach (var entity in entitiesWithDeadlines)
                {
                    entity.Status = "Terminé";
                    
                }
                dbContext.SaveChanges();
            }
            await System.Threading.Tasks.Task.Delay(12000, stoppingToken);
        }
    }

}
