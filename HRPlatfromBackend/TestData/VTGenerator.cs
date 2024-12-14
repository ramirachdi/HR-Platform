using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class VTGenerator
    {
        public Faker<ValidationTask> fakeVT;
        private readonly List<Guid> _userIds;
        private readonly List<Guid> _tasksIds;

        private readonly AppDbContext _db;

        public VTGenerator(AppDbContext db)
        {
            _db = db;
            _userIds = _db.Users.Select(u => u.Id).ToList();
            _tasksIds = _db.Tasks.Select(u => u.Id).ToList();

            fakeVT = new Faker<ValidationTask>()

           .RuleFor(t => t.Validation, f => f.Random.Bool())
           .RuleFor(t => t.Cause, f => f.Lorem.Paragraph())
           .RuleFor(t => t.UserId, f => f.PickRandom(_userIds))
           .RuleFor(t => t.TaskId, f => f.PickRandom(_tasksIds));

        }

        public List<ValidationTask> GenerateFakeVTs(int n)
        {
            return fakeVT.Generate(n);
        }
    }
}
