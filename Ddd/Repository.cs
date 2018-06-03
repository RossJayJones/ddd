using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddd
{
    public abstract class Repository<TAggregate, TAggregateId> : IRepository<TAggregate, TAggregateId>
        where TAggregate : Aggregate<TAggregateId>
        where TAggregateId : Identity
    {
        private readonly IDomainUnitOfWork _unitOfWork;

        protected Repository(IDomainUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TAggregate> Load(TAggregateId id)
        {
            var aggregate = await DoLoad(id).ConfigureAwait(false);
            _unitOfWork.Register(aggregate);
            return aggregate;
        }

        public Task<IReadOnlyDictionary<TAggregateId, TAggregate>> Load(IEnumerable<TAggregateId> ids)
        {
            return DoLoad(ids);
        }

        public async Task Add(TAggregate aggregate)
        {
            await DoAdd(aggregate);
            _unitOfWork.Register(aggregate);
        }

        public void Remove(TAggregate aggregate)
        {
            DoRemove(aggregate);
        }

        protected abstract Task<TAggregate> DoLoad(TAggregateId id);

        protected abstract Task<IReadOnlyDictionary<TAggregateId, TAggregate>> DoLoad(IEnumerable<TAggregateId> ids);

        protected abstract Task DoAdd(TAggregate aggregate);

        protected abstract void DoRemove(TAggregate aggregate);
    }
}
