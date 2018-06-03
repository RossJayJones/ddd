using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ddd.EntityFramework
{
    public abstract class EntityFrameworkRepository<TDbContext, TAggregate, TAggregateId> : Repository<TAggregate, TAggregateId>
        where TDbContext : DbContext
        where TAggregate : Aggregate<TAggregateId>
        where TAggregateId : Identity
    {
        protected EntityFrameworkRepository(TDbContext dbContext, IDomainUnitOfWork unitOfWork) : base(unitOfWork)
        {
            DbContext = dbContext;
        }
        
        protected TDbContext DbContext { get; }

        protected override Task DoAdd(TAggregate aggregate)
        {
            return DbContext.AddAsync(aggregate);
        }

        protected override void DoRemove(TAggregate aggregate)
        {
            DbContext.Remove(aggregate);
        }
    }
}
