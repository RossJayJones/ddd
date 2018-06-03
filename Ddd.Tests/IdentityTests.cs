using System;
using Xunit;

namespace Ddd.Tests
{
    public class IdentityTests
    {
        [Fact]
        public void ItShouldBeTrueWhenComparedToSameString()
        {
            var identity = new Identity("a");
            Assert.True(identity.Equals("a"));
        }

        [Fact]
        public void ItShouldBeFalseWhenComparedToDifferentString()
        {
            var identity = new Identity("a");
            Assert.False(identity.Equals("b"));
        }

        [Fact]
        public void ItShouldBeTrueWhenComparedToSameIdentity()
        {
            var identity1 = new Identity("a");
            var identity2 = new Identity("a");
            Assert.True(identity1.Equals(identity2));
            Assert.True(identity1 == identity2);
        }

        [Fact]
        public void ItShouldBeFalseWhenComparedToDifferentIdentity()
        {
            var identity1 = new Identity("a");
            var identity2 = new Identity("b");
            Assert.False(identity1.Equals(identity2));
            Assert.False(identity1 == identity2);
        }

        [Fact]
        public void ItShouldBeTrueWhenNotOperatorIsUsed()
        {
            var identity1 = new Identity("a");
            var identity2 = new Identity("b");
            Assert.False(identity1.Equals(identity2));
            Assert.True(identity1 != identity2);
        }
    }
}
