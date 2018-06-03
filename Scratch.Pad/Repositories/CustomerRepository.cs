using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ddd;

namespace Scratch.Pad.Repositories
{
    public class CustomerRepository : Repository<Customer, CustomerId>
    {
        public CustomerRepository(IDomainBehaviour unitOfWork) : base(unitOfWork)
        {
        }

        protected override Task<Customer> DoLoad(CustomerId id)
        {
            throw new NotImplementedException();
        }

        protected override Task<IReadOnlyDictionary<CustomerId, Customer>> DoLoad(IEnumerable<CustomerId> ids)
        {
            throw new NotImplementedException();
        }

        protected override void DoAdd(Customer aggregate)
        {
            throw new NotImplementedException();
        }

        protected override void DoRemove(Customer aggregate)
        {
            throw new NotImplementedException();
        }
    }
}
