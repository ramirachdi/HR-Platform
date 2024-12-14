
using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;
using System.Reflection.Emit;

namespace ProjetNET.TestData
{
    public class BlameGenerator
    {
        Faker<Blame> fakeBlame;
        private readonly List<Guid> _userIds;

        private readonly AppDbContext _db;


        public BlameGenerator(AppDbContext db)
        {
            _db = db;
            _userIds = db.Users.Select(u => u.Id).ToList();
            fakeBlame = new Faker<Blame>()
           .RuleFor(t => t.Id, f => Guid.NewGuid())
           .RuleFor(t => t.Object, f => f.Lorem.Paragraph())
           .RuleFor(t => t.Contention, f => f.Lorem.Paragraph())
           .RuleFor(t => t.Name, f => f.Lorem.Sentence())
               .RuleFor(t => t.UserId, f => f.PickRandom(_userIds));

        }
        public List<Blame> GenerateFakeBlames(int n)
        {
            return fakeBlame.Generate(n);

        }
    }
}
