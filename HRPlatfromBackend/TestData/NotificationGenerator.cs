using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;
using System.Reflection.Emit;

namespace ProjetNET.TestData
{
    public class NotificationGenerator
    {
        Faker<Notification> fakeNoti;
        private readonly List<Guid> _userIds;

        private readonly AppDbContext _db;


        public NotificationGenerator(AppDbContext db)
        {
            _db = db;
            _userIds = db.Users.Select(u => u.Id).ToList();
            fakeNoti = new Faker<Notification>()
           .RuleFor(t => t.Id, f => Guid.NewGuid())
           .RuleFor(t => t.Title, f => f.Lorem.Sentence())
           .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
           .RuleFor(t => t.Date, f => f.Date.Recent())
               .RuleFor(t => t.UserId, f => f.PickRandom(_userIds));

        }
        public List<Notification> GenerateFakeNotifications(int n)
        {
            return fakeNoti.Generate(n);

        }
    }
}
