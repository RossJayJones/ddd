using System.Collections.Generic;
using System.Threading.Tasks;
using Ddd;
using Ddd.Queries;
using Sample.Domain.People;
using Sample.Domain.People.Events;
using Sample.Domain.Queries;
using Sample.Domain.Queries.Model;

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

        private Person()
        {
            _contacts = new List<Contact>();
        }

        public Person(Name name) : this()
        {
            Name = name;
        }

        public Name Name { get; set; }

        public IReadOnlyCollection<Contact> Contacts => _contacts;

        public void ChangeName(Name name)
        {
            Name = name;
            AddDomainEvent(new NameChangedDomainEvent(Id, Name));
        }

        public Task<PagedCollection<PersonByName>> FindOtherPeopleWithTheSameName()
        {
            var query = new FindPeopleByName {Term = Name.Full};
            return DomainQueryDispatcher.Execute(query);
        }
    }
}
