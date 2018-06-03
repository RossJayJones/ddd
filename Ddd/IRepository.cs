using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddd
{
    public interface IRepository<TAggregate>
    {
        Task Add(TAggregate aggregate);

        void Remove(TAggregate aggregate);
    }

    public interface IRepository<TAggreagate, TAggregateId> : IRepository<TAggreagate>
        where TAggreagate : Aggregate<TAggregateId>
        where TAggregateId : Identity
    {
        Task<TAggreagate> Load(TAggregateId id);

        Task<IReadOnlyDictionary<TAggregateId, TAggreagate>> Load(IEnumerable<TAggregateId> ids);
    }
}
