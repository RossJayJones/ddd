using Xunit;

namespace Ddd.Tests.AggregateTests
{
    public class EqualityTests
    {
        [Fact]
        public void TheyShouldBeEqualWhenTheyHaveTheSameIdentity()
        {
            var aggregate1 = new MyFirstAggregate(new MyFirstAggregateId("a"));
            var aggregate2 = new MyFirstAggregate(new MyFirstAggregateId("a"));
            Assert.True(aggregate1.Equals(aggregate2));
        }

        [Fact]
        public void TheyShouldNotBeEqualWhenTheyHaveDifferentIdentity()
        {
            var aggregate1 = new MyFirstAggregate(new MyFirstAggregateId("a"));
            var aggregate2 = new MyFirstAggregate(new MyFirstAggregateId("b"));
            Assert.False(aggregate1.Equals(aggregate2));
        }

        [Fact]
        public void TheyShouldNotBeEqualWhenTheyAreDifferentTypes()
        {
            var aggregate1 = new MyFirstAggregate(new MyFirstAggregateId("a"));
            var aggregate2 = new MySecondAggregate(new MySecondAggregateId("a"));
            Assert.False(aggregate1.Equals(aggregate2));
        }

        public class MyFirstAggregateId : Identity
        {
            public MyFirstAggregateId(string value) : base(value)
            {
                
            }
        }

        public class MyFirstAggregate : Aggregate<MyFirstAggregateId>
        {
            public MyFirstAggregate(MyFirstAggregateId id)
            {
                Id = id;
            }
        }

        public class MySecondAggregateId : Identity
        {
            public MySecondAggregateId(string value) : base(value)
            {
                
            }
        }

        public class MySecondAggregate : Aggregate<MySecondAggregateId>
        {
            public MySecondAggregate(MySecondAggregateId id)
            {
                Id = id;
            }
        }
    }
}
