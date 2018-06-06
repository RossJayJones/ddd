using Ddd;
using Ddd.Queries;
using Sample.Domain.Queries.Model;

namespace Sample.Domain.Queries
{
    public class FindPeopleByName : IDomainQuery<PagedCollection<PersonByName>>
    {
        public string Term { get; set; }
    }
}
