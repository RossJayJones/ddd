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

        public TeamId TeamId { get; set; }

        public IReadOnlyCollection<Contact> Contacts => _contacts;

        public void Update(Name name)
        {
            Name = name;
            AddDomainEvent(new PersonUpdatedDomainEvent(Id, Name));
        }

        public void AddContact(Contact contact)
        {
            if (_contacts.Contains(contact))
            {
                return;
            }

            _contacts.Add(contact);
        }

        public void RemoveContact(Contact contact)
        {
            if (!_contacts.Contains(contact))
            {
                return;
            }

            _contacts.Remove(contact);
        }

        public Task<PagedCollection<PersonByName>> FindOtherPeopleWithTheSameName()
        {
            var query = new FindPeopleByName {Term = Name.Full};
            return DomainQueryDispatcher.Execute(query);
        }

        public void Delete()
        {
            AddDomainEvent(new PersonDeletedDomainEvent(Id));
        }

        public bool BelongsToTeam()
        {
            return TeamId != null;
        }
    }
}
