using System.Threading.Tasks;
using Sample.Domain.People;

namespace Sample.Application.InfrastructureServices
{
    public interface IEmailDispatcherService
    {
        Task Dispatch(Email to, string body);
    }
}
