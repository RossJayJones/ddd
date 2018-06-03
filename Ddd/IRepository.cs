using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddd
{
    public interface IRepository<TAggregate>
    {
        Task Add(TAggregate aggregate);

        void Remove(TAggregate aggregate);

        Task<TAggregate> Load(Identity id);

        Task<IReadOnlyDictionary<Identity, TAggregate>> Load(IEnumerable<Identity> ids);
    }
}
