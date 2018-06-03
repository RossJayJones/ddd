using System.Threading;
using System.Threading.Tasks;
using Ddd;
using Scratch.Pad.DomainEvents;

namespace Scratch.Pad.DomainHandlers
{
    public class PersonCreatedDomainEventHandler : IDomainEventHandler<PersonCreatedDomainEvent>
    {
        public Task Handle(PersonCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
