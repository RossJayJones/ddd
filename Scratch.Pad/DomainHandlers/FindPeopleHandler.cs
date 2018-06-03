using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Microsoft.EntityFrameworkCore;
using Scratch.Pad.Data;
using Scratch.Pad.DomainQueries;

namespace Scratch.Pad.DomainHandlers
{
    public class FindPeopleHandler : IDomainQueryHandler<FindPeople, IEnumerable<Person>>
    {
        private readonly MyDbContext _db;

        public FindPeopleHandler(MyDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Person>> Handle(FindPeople request, CancellationToken cancellationToken)
        {
            var response = await _db.People.ToListAsync().ConfigureAwait(false);
            return response;

        }
    }
}
