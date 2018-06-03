using MediatR;

namespace Ddd
{
    public interface IDomainQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
