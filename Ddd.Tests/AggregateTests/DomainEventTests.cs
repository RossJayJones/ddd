using System.Linq;
using Xunit;

namespace Ddd.Tests.AggregateTests
{
    public class DomainEventTests
    {
        [Fact]
        public void ItShouldManageDomainEvents()
        {
            var aggregate = new MyAggregate();
            aggregate.DoSomething();
            var domainEvents = aggregate.FlushDomainEvents().ToList();
            Assert.Equal(1, domainEvents.Count);
            domainEvents = aggregate.FlushDomainEvents().ToList();
            Assert.Equal(0, domainEvents.Count);
        }

        public class MyAggregateId : Identity
        {
            public MyAggregateId(string value) : base(value)
            {
            }
        }

        public class MyAggregate : Aggregate<MyAggregateId>
        {
            public void DoSomething()
            {
                AddDomainEvent(new MyDomainEvent());
            }   
        }

        public class MyDomainEvent : IDomainEvent
        {
            
        }
    }
}
