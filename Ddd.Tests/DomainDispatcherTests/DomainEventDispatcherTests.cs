using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Ddd.Autofac;
using Xunit;

namespace Ddd.Tests.DomainDispatcherTests
{
    public class DomainEventDispatcherTests
    {
        [Fact]
        public async Task ItShouldDispatchDomainEventsToSubscribers()
        {
            var counter = new PublishedEventsCounter();
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> { GetType().Assembly }, c =>
            {
                c.Register(_ => counter);
            });
            var dispatcher = container.Resolve<IDomainEventDispatcher>();
            var aggregate = new MyAggreagte();
            aggregate.DoSomething();
            await dispatcher.Dispatch(aggregate);
            Assert.Equal(2, counter.Count);
        }

        [Fact]
        public async Task ItShouldNotFailWhenThereAreNoSubscribers()
        {
            var counter = new PublishedEventsCounter();
            var container = AutofacConfiguration.CreateContainer(new List<Assembly> { GetType().Assembly }, c =>
            {
                c.Register(_ => counter);
            });
            var dispatcher = container.Resolve<IDomainEventDispatcher>();
            var aggregate = new MyAggreagte();
            aggregate.DoSomethingWhichIsNotBeingHanlded();
            await dispatcher.Dispatch(aggregate);
            Assert.Equal(0, counter.Count);
        }

        public class MyAggregateId : Identity
        {
            public MyAggregateId(string value) : base(value)
            {
            }
        }

        public class MyAggreagte : Aggregate<MyAggregateId>
        {
            public void DoSomething()
            {
                AddDomainEvent(new SomethingHappenedDomainEvent());
            }

            public void DoSomethingWhichIsNotBeingHanlded()
            {
                AddDomainEvent(new SomethignHappenedWhichIsNotBeingHandled());
            }
        }

        public class SomethingHappenedDomainEvent : IDomainEvent
        {
            
        }

        public class SomethignHappenedWhichIsNotBeingHandled : IDomainEvent
        {
            
        }

        public class PublishedEventsCounter
        {
            public int Count { get; private set; }

            public void Increment()
            {
                Count++;
            }
        }

        public class FirstHandler : IDomainEventHandler<SomethingHappenedDomainEvent>
        {
            private readonly PublishedEventsCounter _counter;

            public FirstHandler(PublishedEventsCounter counter)
            {
                _counter = counter;
            }

            public Task Handle(SomethingHappenedDomainEvent notification, CancellationToken cancellationToken)
            {
                _counter.Increment();
                return Task.CompletedTask;
            }
        }

        public class SecondHandler : IDomainEventHandler<SomethingHappenedDomainEvent>
        {
            private readonly PublishedEventsCounter _counter;

            public SecondHandler(PublishedEventsCounter counter)
            {
                _counter = counter;
            }

            public Task Handle(SomethingHappenedDomainEvent notification, CancellationToken cancellationToken)
            {
                _counter.Increment();
                return Task.CompletedTask;
            }
        }
    }
}
