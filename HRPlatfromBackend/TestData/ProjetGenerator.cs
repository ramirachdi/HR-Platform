using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class ProjetGenerator
    {
        Faker<Projet> fakeProject;
        private readonly AppDbContext _db;


        public ProjetGenerator(AppDbContext db)
        {
            _db = db;
            fakeProject = new Faker<Projet>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Name, f => f.Lorem.Word())
            .RuleFor(t => t.Description, f => f.Lorem.Sentence())
            .RuleFor(t => t.Status, f => f.PickRandom("En cours", "Terminée", "En attente"))
            .RuleFor(t => t.Users, f => {
                // Fetch a random number of users from the database
                var numberOfUsers = f.Random.Number(1, 5);
                var randomUsers = _db.Users.OrderBy(u => Guid.NewGuid()).Take(numberOfUsers).ToList();

                return randomUsers;
            });
        }

        public List<Projet> GenerateFakeProjets(int n)
        {
            return fakeProject.Generate(n);

        }
    }
}
