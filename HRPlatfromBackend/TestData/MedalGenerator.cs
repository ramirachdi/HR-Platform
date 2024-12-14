
using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;
using System.Reflection.Emit;

namespace ProjetNET.TestData
    {
        public class MedalGenerator
        {
            Faker<Medal> fakeMedal;
            private readonly List<Guid> _userIds;

            private readonly AppDbContext _db;


            public MedalGenerator(AppDbContext db)
            {
                _db = db;
                _userIds = db.Users.Select(u => u.Id).ToList();
            fakeMedal = new Faker<Medal>()
           .RuleFor(t => t.Id, f => Guid.NewGuid())
           .RuleFor(t => t.Name, f => f.Lorem.Sentence())
           .RuleFor(t => t.Type, f => f.PickRandom("Membre du mois", "Innovation","0 absences"))
           .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
           .RuleFor(t => t.Date, f => f.Date.Recent())
           .RuleFor(t => t.UserId, f => f.PickRandom(_userIds));

            }
            public List<Medal> GenerateFakeMedals(int n)
            {
                return fakeMedal.Generate(n);

            }
        }
    }

   