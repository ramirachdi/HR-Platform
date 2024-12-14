using Bogus;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class MeetingGenerator
    {
        Faker<Meeting> fakeMeeting;
        private readonly AppDbContext _db;


        public MeetingGenerator(AppDbContext db)
        {
            _db = db;
            fakeMeeting = new Faker<Meeting>()
              .RuleFor(m => m.Id, f => Guid.NewGuid())
              .RuleFor(m => m.Name, f => f.Lorem.Word())
              .RuleFor(m => m.Description, f => f.Lorem.Sentence())
              .RuleFor(m => m.Type, f => f.PickRandom("AG", "RG", "Workshop"))
              .RuleFor(m => m.Location, f => f.PickRandom("Local Enactus", "salle " + f.Random.Number(100, 300)))
              .RuleFor(m => m.Date, f => f.Date.Future())
              .RuleFor(m => m.Users, f => {
                  // Fetch a random number of users from the database
                  var numberOfUsers = f.Random.Number(20, 50);
                  var randomUsers = _db.Users.OrderBy(u => Guid.NewGuid()).Take(numberOfUsers).ToList();

                  return randomUsers;
              });

        }
        public List<Meeting> GenerateFakeMeetings(int n)
        {
            return fakeMeeting.Generate(n);

        }
    }
}
