using Ddd;

namespace Sample.Domain.People.Events
{
    public class PersonUpdatedDomainEvent : IDomainEvent
    {
        public PersonUpdatedDomainEvent(PersonId personId, Name name)
        {
            PersonId = personId;
            Name = name;
        }

        public PersonId PersonId { get; }

        public Name Name { get; }
    }
}
