using System.Collections.Generic;
using Ddd;
using Sample.Domain.People;

namespace Sample.Domain
{
    public class PersonId : Identity
    {
        public PersonId(string value) : base(value)
        {
        }
    }

    public class Person : Aggregate<PersonId>
    {
        private readonly List<Contact> _contacts;

        public Person()
        {
            _contacts = new List<Contact>();
        }

        public Name Name { get; set; }

        public IReadOnlyCollection<Contact> Contacts => _contacts;
    }
}
