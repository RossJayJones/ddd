using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddd
{
    public class DomainUnitOfWork : IDomainUnitOfWork
    {
        private readonly IEnumerable<IDomainBehaviour> _behaviours;

        public DomainUnitOfWork(IEnumerable<IDomainBehaviour> beahviours)
        {
            _behaviours = beahviours;
        }

        public void Register(IAggregate item)
        {
            foreach (var domainBehaviour in _behaviours)
            {
                domainBehaviour.Register(item);
            }
        }

        public async Task Commit()
        {
            foreach (var domainBehaviour in _behaviours)
            {
                await domainBehaviour.Commit().ConfigureAwait(false);
            }
        }
    }
}
