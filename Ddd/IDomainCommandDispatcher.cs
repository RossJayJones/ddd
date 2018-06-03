using System.Threading.Tasks;

namespace Ddd
{
    public interface IDomainCommandDispatcher
    {
        Task Dispatch(IDomainCommand command);

        Task<TResponse> Dispatch<TResponse>(IDomainCommand<TResponse> command);
    }
}
