using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ddd;

namespace Sample.Domain
{
    public class MyCustomPersistenceRepository<TAggregate, TAggregateId> : Repository<TAggregate, TAggregateId>
        where TAggregate : Aggregate<TAggregateId>
        where TAggregateId : Identity
    {
        public MyCustomPersistenceRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        protected override Task<TAggregate> DoLoad(Identity id)
        {
            throw new NotImplementedException();
        }

        protected override Task<IReadOnlyDictionary<Identity, TAggregate>> DoLoad(IEnumerable<Identity> ids)
        {
            throw new NotImplementedException();
        }

        protected override Task DoAdd(TAggregate aggregate)
        {
            throw new NotImplementedException();
        }

        protected override void DoRemove(TAggregate aggregate)
        {
            throw new NotImplementedException();
        }
    }

    public class PersonRepository : MyCustomPersistenceRepository<Person, PersonId>
    {
        public PersonRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
