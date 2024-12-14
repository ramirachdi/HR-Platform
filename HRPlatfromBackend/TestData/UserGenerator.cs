using Bogus;
using Microsoft.EntityFrameworkCore;
using ProjetNET.Models;


namespace ProjetNET.TestData
{
    public class UserGenerator
    {
       public Faker<User> fakeUser;

        public UserGenerator()
        {
            fakeUser = new Faker<User>()
               .RuleFor(u => u.Id, f => Guid.NewGuid())
               .RuleFor(u => u.FirstName, f => f.Person.FirstName)
               .RuleFor(u => u.LastName, f => f.Person.LastName)
               .RuleFor(u => u.Email, f => f.Internet.Email())
               .RuleFor(u => u.Adress, f => f.Address.FullAddress())
               .RuleFor(u => u.IsAdmin, f => f.Random.Bool())
               .RuleFor(u => u.Status, f => f.PickRandom("MembreActif"))
               .RuleFor(u => u.UserName, f => f.Internet.UserName())
               .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
               .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());
        }

        public List<User> GenerateFakeUsers(int n)
        {
            return fakeUser.Generate(n);

        }
    }

}
