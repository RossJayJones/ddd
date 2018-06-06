using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ddd;
using Sample.Domain;

namespace Sample.Application.Repositories
{
    public class PersonRepository : Repository<Person, PersonId>
    {
        public PersonRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override Task<Person> DoLoad(Identity id)
        {
            throw new NotImplementedException();
        }

        protected override Task<IReadOnlyDictionary<Identity, Person>> DoLoad(IEnumerable<Identity> ids)
        {
            throw new NotImplementedException();
        }

        protected override Task DoAdd(Person aggregate)
        {
            throw new NotImplementedException();
        }

        protected override void DoRemove(Person aggregate)
        {
            throw new NotImplementedException();
        }
    }
}
