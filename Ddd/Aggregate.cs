using System.Collections.Generic;

namespace Ddd
{
    public class Aggregate<TIdentity> : Entity<TIdentity>, IAggregate
        where TIdentity : Identity
    {
        private readonly List<IDomainEvent> _domainEvents;

        public Aggregate()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        public IEnumerable<IDomainEvent> FlushDomainEvents()
        {
            foreach (var domainEvent in _domainEvents)
            {
                yield return domainEvent;
            }

            _domainEvents.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }
    }
}
