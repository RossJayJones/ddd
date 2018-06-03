using Ddd;

namespace Scratch.Pad.DomainEvents
{
    public class PersonCreatedDomainEvent : IDomainEvent
    {
        public Person Person { get; }

        public PersonCreatedDomainEvent(Person person)
        {
            Person = person;
        }
    }
}
