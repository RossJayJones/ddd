using System;
using Ddd;
using Scratch.Pad.DomainEvents;

namespace Scratch.Pad
{
    public class CustomerId : Identity
    {
        public CustomerId(string value) : base(value)
        {
            
        }

        public CustomerId() : base(Guid.NewGuid().ToString())
        {
            
        }
    }

    public class Customer : Aggregate<CustomerId>
    {
        public Customer(CustomerId id, string name)
        {
            Id = id;
            Name = name;
            AddDomainEvent(new CustomerCreatedDomainEvent(this));
        }

        public string Name { get; private set; }
    }
}
