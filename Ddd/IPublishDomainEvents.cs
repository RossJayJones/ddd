using System.Collections.Generic;

namespace Ddd
{
    public interface IPublishDomainEvents
    {
        IEnumerable<IDomainEvent> FlushDomainEvents();

        void AddDomainEvent(IDomainEvent domainEvent);
    }
}
