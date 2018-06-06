using System.Linq;
using Ddd.Queries;
using Xunit;

namespace Ddd.Tests.Queries
{
    public class PagedCollectionTests
    {
        [Theory]
        [InlineData(25, 0, 1)]
        [InlineData(25, 25, 2)]
        [InlineData(25, 75, 4)]
        [InlineData(100, 200, 3)]
        public void ItShouldCalculateThePageCorrectly(int take, int skip, int expectedPage)
        {
            var data = Enumerable.Range(0, take).ToList();
            var collection = new PagedCollection<int>(data, 1000, take, skip);
            Assert.Equal(expectedPage, collection.Page);
        }
    }
}
