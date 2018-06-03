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
        /// <summary>
        /// Required for entity framework persistence :/
        /// </summary>
        private Person()
        {
            
        }

        public Person(PersonId id, string firstName, string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            AddDomainEvent(new PersonCreatedDomainEvent(this));
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }
    }
}
