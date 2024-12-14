
using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class HPGenerator
    {
        public Faker<HistoriquePresence> fakeHP;
        private readonly List<Guid> _userIds;
        private readonly List<Guid> _meetingsIds;

        private readonly AppDbContext _db;

        public HPGenerator(AppDbContext db)
        {
            _db = db;
            _userIds = _db.Users.Select(u => u.Id).ToList();
            _meetingsIds = _db.Meetings.Select(u => u.Id).ToList();

            fakeHP = new Faker<HistoriquePresence>()

           .RuleFor(t => t.Presence, f => false)
           .RuleFor(t => t.Confirmed, f => false)
           .RuleFor(t => t.Denied, f => false)

           .RuleFor(t => t.Cause, f => f.Lorem.Paragraph())
           .RuleFor(t => t.UserId, f => f.PickRandom(_userIds))
           .RuleFor(t => t.MeetingId, f => f.PickRandom(_meetingsIds));

        }

        public List<HistoriquePresence> GenerateFakeHPs(int n)
        {
            return fakeHP.Generate(n);
        }
    }
}
