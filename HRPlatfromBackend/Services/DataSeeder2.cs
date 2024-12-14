
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
        public class DataSeeder2 : IHostedService
        {
            private readonly ILogger<DataSeeder> _logger;
            private readonly IServiceProvider _serviceProvider;

            public DataSeeder2(ILogger<DataSeeder> logger, IServiceProvider serviceProvider)
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



                var fakeTasks = new TaskGenerator(scopedContext).GenerateFakeTasks(20);
                scopedContext.Tasks.AddRange(fakeTasks);

                var fakeProjects = new ProjetGenerator(scopedContext).GenerateFakeProjets(7);
                scopedContext.Projets.AddRange(fakeProjects);

                var fakeMeetings = new MeetingGenerator(scopedContext).GenerateFakeMeetings(10);
                scopedContext.Meetings.AddRange(fakeMeetings);

                var fakeEvents = new EventGenerator().GenerateFakeEvents(10);
                scopedContext.Events.AddRange(fakeEvents);

                var fakeNotis = new NotificationGenerator(scopedContext).GenerateFakeNotifications(250);
                scopedContext.Notifications.AddRange(fakeNotis);

                var fakeMedals = new MedalGenerator(scopedContext).GenerateFakeMedals(20);
                scopedContext.Medals.AddRange(fakeMedals);


                var fakeEquipes = new ChildEquipeGenerator(scopedContext).GenerateFakeEquipes(5);
                scopedContext.Equipes.AddRange(fakeEquipes);

                var fakeBlames = new BlameGenerator(scopedContext).GenerateFakeBlames(20);
                scopedContext.Blames.AddRange(fakeBlames);

               // var fakeAnonymBoxComment = new AnonymBoxGenerator().GenerateFakeAnonymBoxComments(10);
                //scopedContext.AnonymBoxComments.AddRange(fakeAnonymBoxComment);



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



