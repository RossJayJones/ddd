using System.Threading.Tasks;

namespace Ddd
{
    public interface IDomainBehaviour
    {
        void Register(IAggregate item);

        Task Commit();
    }
}
