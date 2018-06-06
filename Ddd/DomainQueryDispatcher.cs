using System;
using System.Threading.Tasks;

namespace Ddd
{
    /// <summary>
    /// Provides a static interface to dispatch queries. This should be performed within a child scope. The Factory function provides
    /// a hook to allow the correct IDomainQueryDispatcher to be resolved within the correct scope.
    /// </summary>
    public static class DomainQueryDispatcher
    {
        public static Func<IDomainQueryDispatcher> Factory;

        public static Task<TResponse> Execute<TResponse>(IDomainQuery<TResponse> query)
        {
            return Factory().Dispatch(query);
        }
    }
}