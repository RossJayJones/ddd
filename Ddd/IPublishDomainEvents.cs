using System.Collections.Generic;

namespace Ddd
{
    public interface IPublishDomainEvents
    {
        IEnumerable<IDomainEvent> FlushDomainEvents();
    }
}
