using Ddd;

namespace Scratch.Pad.DomainEvents
{
    public class CustomerCreatedDomainEvent : IDomainEvent
    {
        public Person Customer { get; }

        public CustomerCreatedDomainEvent(Person customer)
        {
            Customer = customer;
        }
    }
}
