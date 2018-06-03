using System.Threading.Tasks;

namespace Ddd
{
    public interface IDomainQueryDispatcher
    {
        Task<TResponse> Dispatch<TResponse>(IDomainQuery<TResponse> query);
    }
}