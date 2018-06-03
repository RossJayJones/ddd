using System.Threading.Tasks;

namespace Ddd
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch(IPublishDomainEvents entity);
    }
}
