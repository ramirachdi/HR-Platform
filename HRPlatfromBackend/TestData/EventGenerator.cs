using Bogus;
using ProjetNET.Models;

namespace ProjetNET.TestData
{
    public class EventGenerator
    {
        public Faker<Event> fakeEvent;

        public EventGenerator()
        {
            fakeEvent = new Faker<Event>()
            .RuleFor(t => t.Id, f => Guid.NewGuid())
            .RuleFor(t => t.Name, f => f.Lorem.Sentence())
            .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
            .RuleFor(t => t.Date, f => f.Date.Recent());
        }

        public List<Event> GenerateFakeEvents(int n)
        {
            return fakeEvent.Generate(n);

        }

    }
}
