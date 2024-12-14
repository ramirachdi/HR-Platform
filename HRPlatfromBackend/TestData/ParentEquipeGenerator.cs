using Bogus;
using ProjetNET.Controllers;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class ParentEquipeGenerator
    {


        Faker<Equipe> fakeEquipe;


        private readonly AppDbContext _db;


        public ParentEquipeGenerator(AppDbContext db)
        {
            _db = db;

            fakeEquipe = new Faker<Equipe>()
                    .RuleFor(t => t.Id, f => Guid.NewGuid())
                    .RuleFor(t => t.Name, f => f.Lorem.Sentence())
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



