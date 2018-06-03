using Ddd;

namespace Scratch.Pad.DomainEvents
{
    public class CustomerCreatedDomainEvent : IDomainEvent
    {
        public Customer Customer { get; }

        public CustomerCreatedDomainEvent(Customer customer)
        {
            Customer = customer;
        }
    }
}
