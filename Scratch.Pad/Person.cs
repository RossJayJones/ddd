using System;
using Ddd;
using Scratch.Pad.DomainEvents;

namespace Scratch.Pad
{
    public class PersonId : Identity
    {
        public PersonId(string value) : base(value)
        {
            
        }

        public PersonId() : base(Guid.NewGuid().ToString())
        {
            
        }
    }

    public class Person : Aggregate<PersonId>
    {
        public Person()
        {
            
        }

        public Person(PersonId id, string name)
        {
            Id = id;
            Name = name;
            AddDomainEvent(new CustomerCreatedDomainEvent(this));
        }

        public string Name { get; private set; }
    }
}
