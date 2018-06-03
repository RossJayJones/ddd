using MediatR;

namespace Ddd
{
    public interface IDomainQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IDomainQuery<TResponse>
    {
    }
}
