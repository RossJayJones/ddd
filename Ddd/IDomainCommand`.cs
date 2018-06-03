using MediatR;

namespace Ddd
{
    public interface IDomainCommand<out TResponse> : IRequest<TResponse>
    {
    }
}
