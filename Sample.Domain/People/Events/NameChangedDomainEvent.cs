using Ddd;

namespace Sample.Domain.People.Events
{
    public class NameChangedDomainEvent : IDomainEvent
    {
        public NameChangedDomainEvent(PersonId personId, Name name)
        {
            PersonId = personId;
            Name = name;
        }

        public PersonId PersonId { get; }

        public Name Name { get; }
    }
}
