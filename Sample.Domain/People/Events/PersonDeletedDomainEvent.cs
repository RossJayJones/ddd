using Ddd;

namespace Sample.Domain.People.Events
{
    public class PersonDeletedDomainEvent : IDomainEvent
    {
        public PersonDeletedDomainEvent(PersonId personId)
        {
            PersonId = personId;
        }

        public PersonId PersonId { get; }
    }
}
