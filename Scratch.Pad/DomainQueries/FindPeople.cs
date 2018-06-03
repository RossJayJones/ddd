using System.Collections.Generic;
using Ddd;

namespace Scratch.Pad.DomainQueries
{
    public class FindPeople : IDomainQuery<IEnumerable<Person>>
    {
        public string Name { get; set; }
    }
}
