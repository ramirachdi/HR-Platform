
using Bogus;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class AnonymBoxGenerator
    {
        public Faker<AnonymBoxComment> fakeAnonymBoxComment;

        public AnonymBoxGenerator()
        {
            fakeAnonymBoxComment = new Faker<AnonymBoxComment>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Contenu, f => f.Lorem.Paragraph())
            .RuleFor(t => t.Date, f => f.Date.Recent());
        }

        public List<AnonymBoxComment> GenerateFakeAnonymBoxComments(int n)
        {
            return fakeAnonymBoxComment.Generate(n);

        }

    }
}
