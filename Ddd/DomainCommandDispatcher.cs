using System;
using System.Threading.Tasks;

namespace Ddd
{
    /// <summary>
    /// Provides a static interface to send domain commands. It is required when sending commands from within aggregates or when it is not
    /// convenient or possible to get an instance of the mediator. The Factory property allows callers to configure where the IDomainCommandDispatcher instance
    /// is sourced from.
    /// </summary>
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
