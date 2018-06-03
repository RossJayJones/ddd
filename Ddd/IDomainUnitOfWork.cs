using System.Threading.Tasks;

namespace Ddd
{
    public interface IDomainUnitOfWork
    {
        void Register(IAggregate item);

        Task Commit();
    }
}
