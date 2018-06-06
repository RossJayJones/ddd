using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Ddd.Autofac;
using Xunit;

namespace Ddd.Tests.RepositoryTests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void ItShouldInjectUnitOfWork()
        {
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> { GetType().Assembly });

            using (var scope = container.BeginLifetimeScope())
            {

            }
        }

        public class MyAggregateId : Identity
        {
            public MyAggregateId() : base(Guid.NewGuid().ToString())
            {
            }
        }

        public class MyAggregate : Aggregate<MyAggregateId>
        {
            
        }

        public class MyRepository : Repository<MyAggregate, MyAggregateId>
        {
            public MyRepository(IDomainUnitOfWork unitOfWork) : base(unitOfWork)
            {
            }

            protected override Task<MyAggregate> DoLoad(Identity id)
            {
                throw new NotImplementedException();
            }

            protected override Task<IReadOnlyDictionary<Identity, MyAggregate>> DoLoad(IEnumerable<Identity> ids)
            {
                throw new NotImplementedException();
            }

            protected override Task DoAdd(MyAggregate aggregate)
            {
                throw new NotImplementedException();
            }

            protected override void DoRemove(MyAggregate aggregate)
            {
                throw new NotImplementedException();
            }
        }
    }
}
