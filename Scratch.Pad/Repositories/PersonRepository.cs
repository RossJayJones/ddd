using System.Collections.Generic;
using System.Threading.Tasks;
using Ddd;
using Ddd.EntityFramework;
using Scratch.Pad.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Scratch.Pad.Repositories
{
    public class PersonRepository : EntityFrameworkRepository<MyDbContext, Person, PersonId>
    {
        public PersonRepository(MyDbContext dbContext, IDomainUnitOfWork unitOfWork) : base(dbContext, unitOfWork)
        {
        }

        protected override Task<Person> DoLoad(Identity id)
        {
            return DbContext.People.SingleOrDefaultAsync(x => x.Id == id);
        }

        protected override async Task<IReadOnlyDictionary<Identity, Person>> DoLoad(IEnumerable<Identity> ids)
        {
            var items = await DbContext.People.Where(x => ids.Contains(x.Id)).ToListAsync();
            return items.Distinct().ToDictionary(x => (Identity)x.Id);
        }
    }
}