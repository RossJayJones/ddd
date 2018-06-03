using MediatR;

namespace Ddd
{
    public interface IDomainCommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : IDomainCommand<TResponse>
    {
    }
}
