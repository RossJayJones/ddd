using System;
using System.Threading.Tasks;

namespace Ddd
{
    public static class DomainCommandDispatcher
    {
        public static Func<IDomainCommandDispatcher> Factory;

        public static Task Send(IDomainCommand command)
        {
            return Factory().Dispatch(command);
        }

        public static Task<TResponse> Send<TResponse>(IDomainCommand<TResponse> command)
        {
            return Factory().Dispatch(command);
        }
    }
}
