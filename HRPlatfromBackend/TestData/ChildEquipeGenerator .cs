using Bogus;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class ChildEquipeGenerator
    {


        Faker<Equipe> fakeEquipe;
        private readonly List<Guid> _equipeIds;


        private readonly AppDbContext _db;


        public ChildEquipeGenerator(AppDbContext db)
        {
            _db = db;
            _equipeIds = db.Equipes.Select(u => u.Id).ToList();

            fakeEquipe = new Faker<Equipe>()
                    .RuleFor(t => t.Id, f => Guid.NewGuid())
                    .RuleFor(t => t.Name, f => f.Lorem.Sentence())
                    .RuleFor(t => t.ParentEquipeId, f => f.PickRandom(_equipeIds))
                    .RuleFor(t => t.Users, f => {
                        // Fetch a random number of users from the database
                        var numberOfUsers = f.Random.Number(1, 5);
                        var randomUsers = _db.Users.OrderBy(u => Guid.NewGuid()).Take(numberOfUsers).ToList();

                        return randomUsers;
                    });

        }
        public List<Equipe> GenerateFakeEquipes(int n)
        {
            return fakeEquipe.Generate(n);

        }
    }
}



