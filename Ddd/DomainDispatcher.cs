using System.Threading.Tasks;
using MediatR;

namespace Ddd
{
    public class DomainDispatcher : IDomainCommandDispatcher, IDomainQueryDispatcher, IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task Dispatch(IDomainCommand command)
        {
            return _mediator.Send(command);
        }

        public Task<TResponse> Dispatch<TResponse>(IDomainCommand<TResponse> command)
        {
            return _mediator.Send(command);
        }

        public Task<TResponse> Dispatch<TResponse>(IDomainQuery<TResponse> query)
        {
            return _mediator.Send(query);
        }

        public async Task Dispatch(IPublishDomainEvents entity)
        {
            foreach (var domainEvent in entity.FlushDomainEvents())
            {
                await _mediator.Publish(domainEvent).ConfigureAwait(false);
            }
        }
    }
}
