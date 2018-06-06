using System.Collections.Generic;

namespace Ddd.Queries
{
    public class PagedCollection<T> : List<T>
    {
        public PagedCollection(IEnumerable<T> data, 
            int total, 
            int take, 
            int skip)
        {
            AddRange(data);
            Total = total;
            Take = take;
            Skip = skip;
        }

        public int Total { get; }
        
        public int Take { get; }

        public int Skip { get; }

        public int Page => Skip / Take + 1;
    }
}
