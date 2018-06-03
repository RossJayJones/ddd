using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ddd.Behaviours
{
    public class DomainEventPublisherBehaviour : IDomainBehaviour
    {
        private readonly List<IPublishDomainEvents> _items;
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DomainEventPublisherBehaviour(IDomainEventDispatcher domainEventDispatcher)
        {
            _items = new List<IPublishDomainEvents>();
            _domainEventDispatcher = domainEventDispatcher;
        }

        public void Register(IAggregate item)
        {
            if (_items.Contains(item))
            {
                return;
            }

            _items.Add(item);
        }

        public async Task Commit()
        {
            foreach (var item in _items)
            {
                await _domainEventDispatcher.Dispatch(item).ConfigureAwait(false);
            }

            _items.Clear();
        }
    }
}