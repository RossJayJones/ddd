using System;
using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Ddd.Queries;
using Sample.Domain.Queries;
using Sample.Domain.Queries.Model;

namespace Sample.Application.DomainQueryHandlers
{
    public class FindPeopleByNameQueryHandler : IDomainQueryHandler<FindPeopleByName, PagedCollection<PersonByName>>
    {
        public Task<PagedCollection<PersonByName>> Handle(FindPeopleByName request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
